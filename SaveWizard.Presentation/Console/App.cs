using SaveWizard.Models;
using SaveWizard.Authorization;
using SaveWizard.Authorization.GitHub;
using Microsoft.Extensions.DependencyInjection;
using SaveWizard.Core.Services;
using SaveWizard.Core.Interfaces;
using System.Threading.Tasks.Sources;
using SaveWizard.Core;
using Microsoft.EntityFrameworkCore;
using SaveWizard.Core.Repositories;

namespace ConsoleWizard;

public class App {
  private WizardUser _user;

  private string _buffer = "";
  private string _token = "";

  public App() {
    _user = new WizardUser();
  }

  public async Task<Task> Run() {
    Console.Write("Enter GitHub Personal Access Token: ");
    _token = Console.ReadLine();

    if (string.IsNullOrEmpty(_token)) {
      Console.WriteLine("Token is empty");
      return Task.CompletedTask;
    }

    SetupServices();
    await Init();
    Render();
    return Task.CompletedTask;
  }

  private async Task<Task> Init() {
    var githubService = ServiceManager.GetService<IGitHubService>();
    var userService = ServiceManager.GetService<IUserService>();
    var encryptionService = ServiceManager.GetService<IEncryptionService>();

    // check if user is registered
    var user = await userService.GetUserByAccessToken(_token);

    if (user == null) {
      _user.CreateGitHubUser(_token);
      var userInfo = await githubService.GetBasicData(_user.UserData!.Client!);
      _user.Email = userInfo.Email;
      _user.Username = userInfo.Login;
      _user.PlatformId = userInfo.Id;
      _user.EnryptionKey = encryptionService.GenerateKey();

      _user = await userService.AddUser(_user);
    } else {
      _user = user;
    }

    return Task.CompletedTask;
  }

  private void Render() {
    while (true) {
      // ConsoleGraphics.Clear();
      ConsoleGraphics.DrawOptions();
      _buffer = Console.ReadLine();
      HandleOption();
    }
  }

  private async void HandleOption() {
    var options = _buffer.Split(" ");
    switch (options[0]) {
      case WizardConstants.Print:
        if (options[1] == WizardConstants.Repos) {
          await PrintRepos();
        } else if (options[1] == WizardConstants.Backups) {
          await PrintBackups();
        } else if (options[1] == WizardConstants.All) {
          await PrintAll();
        }
        break;
      case WizardConstants.Select:
        if (options[1] == WizardConstants.Repo) {
          SelectRepo(options[2]);
        } else if (options[1] == WizardConstants.Backup) {
          SelectBackup(options[2]);
        }
        break;
      case WizardConstants.Delete:
        await RemoveBackup(options[1]);
        break;
      case WizardConstants.Clear:
        ConsoleGraphics.Clear();
        break;
      default:
        Console.WriteLine("Unknown Command");
        break;
    }
  }

  private async Task<Task> RemoveBackup(string id) {
    var backupService = ServiceManager.GetService<IBackupService>();
    var ioService = ServiceManager.GetService<IIOService>();

    var guid = Guid.Parse(id);
    var backupInfo = await backupService.GetBackupById(guid);

    if (backupInfo == null) {
      return Task.CompletedTask;
    }

    await backupService.RemoveBackup(guid);
    ioService.RemoveFromDisk(backupInfo.Filename!);

    return Task.CompletedTask;
  }

  private async Task<Task> PrintAll() {
    await PrintRepos();
    await PrintBackups();
    return Task.CompletedTask;
  }

  private async Task<Task> PrintRepos() {
    var githubService = ServiceManager.GetService<IGitHubService>();

    var repos = await githubService.GetAllPrivateRepositories(_user);
    foreach (var repo in repos) {
      Console.WriteLine($"[R] [{repo.Id}] {repo.Name}");
    }
    return Task.CompletedTask;
  }

  private async Task<Task> PrintBackups() {
    var ioService = ServiceManager.GetService<IIOService>();
    var backupService = ServiceManager.GetService<IBackupService>();

    var files = ioService.GetBackups().ToArray();
    var backups = await backupService.GetLatestBackupsByUserId(_user.WizardId);

    foreach (var backup in backups) {
      var fileTarget = files.Where(x => x == backup.Filename).FirstOrDefault();
      if (fileTarget == null) {
        continue;
      }

      Console.WriteLine($"[B] [{backup.Id}] [{backup.RepositoryName}] [{fileTarget}]");
    }

    return Task.CompletedTask;
  }

  private async void SelectBackup(string id) {
    var ioService = ServiceManager.GetService<IIOService>();
    var encryptionService = ServiceManager.GetService<IEncryptionService>();
    var backupService = ServiceManager.GetService<IBackupService>();
    var githubService = ServiceManager.GetService<IGitHubService>();

    var guid = Guid.Parse(id);
    var result = await backupService.GetBackupById(guid);

    var file = ioService.LoadFile(result.Filename!);
    var data = encryptionService.DecryptData<BackupData>(file, _user.EnryptionKey!);

    await githubService.AddIssues(_user, data.WizardIssues, data.RepositoryId);
  }

  private async void SelectRepo(string id) {
    var githubService = ServiceManager.GetService<IGitHubService>();
    var encryptionService = ServiceManager.GetService<IEncryptionService>();
    var ioService = ServiceManager.GetService<IIOService>();
    var backupService = ServiceManager.GetService<IBackupService>();

    long.TryParse(id, out var repoId);
    if (repoId == 0) {
      return;
    }

    var repoInfo = await githubService.SelectRepository(_user, repoId);
    foreach (var issue in repoInfo.Issues) {
      Console.WriteLine($"[{issue.Id}] {issue.Title}");
    }

    var backup = new BackupData();
    backup.RepositoryId = repoId;
    backup.WizardIssues = repoInfo.Issues;

    var encrypted = encryptionService.EncryptData(backup, _user.EnryptionKey!);
    var date = DateTime.Now.ToString("yyyyMMddTHHmmss");
    var filename = $"{repoInfo.RepositoryName}-{date}.wiz";

    var dbBackup = new DbBackupRecord();
    dbBackup.Id = Guid.NewGuid();
    dbBackup.UserId = _user.WizardId;
    dbBackup.RepositoryName = repoInfo.RepositoryName;
    dbBackup.Date = DateTime.Now;
    dbBackup.Filename = filename;
    await backupService.AddBackup(dbBackup);

    ioService.SaveFile(encrypted, filename);
  }

  private void SetupServices() {
    ServiceManager.Services.ConfigureAll(typeof(IWizardService), typeof(IWizardDbContext));
    ServiceManager.SetProvider(ServiceManager.Services);

    var db = ServiceManager.GetService<WizardContext>();
    db.Database.Migrate();
  }
}

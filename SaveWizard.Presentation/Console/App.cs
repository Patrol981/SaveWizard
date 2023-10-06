using SaveWizard.Models;
using SaveWizard.Authorization;
using SaveWizard.Authorization.GitHub;
using Microsoft.Extensions.DependencyInjection;
using SaveWizard.Core.Services;
using SaveWizard.Core.Interfaces;
using System.Threading.Tasks.Sources;
using KizerSharp.Services;
using SaveWizard.Core;
using Microsoft.EntityFrameworkCore;
using SaveWizard.Core.Repositories;

namespace ConsoleWizard;

public class App {
  private WizardUser _user;

  private string _buffer = "";
  // private string _key = "00112233445566778899AABBCCDDEEFF";
  private string _token = "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";

  public App() {
    _user = new WizardUser();
  }

  public async Task<Task> Run() {
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

      await userService.AddUser(_user);
    } else {
      _user = user;
    }

    return Task.CompletedTask;
  }

  private void Render() {
    while (true) {
      ConsoleGraphics.Clear();
      ConsoleGraphics.DrawOptions();
      _buffer = Console.ReadLine();
      HandleOption();
    }
  }

  private void HandleOption() {
    var options = _buffer.Split(" ");
    switch (options[0]) {
      case WizardConstants.Print:
        Print();
        break;
      case WizardConstants.Select:
        Select(options[1]);
        break;
      case WizardConstants.Read:
        ReadFile(options[1]);
        break;
      case WizardConstants.Clear:
        ConsoleGraphics.Clear();
        break;
      default:
        Console.WriteLine("Unknown Command");
        break;
    }
  }

  private async void Print() {
    var githubService = ServiceManager.GetService<IGitHubService>();

    var repos = await githubService.GetAllPrivateRepositories(_user);
    foreach (var repo in repos) {
      Console.WriteLine($"[{repo.Id}] {repo.Name}");
    }
  }

  private async void Select(string id) {
    var githubService = ServiceManager.GetService<IGitHubService>();
    var encryptionService = ServiceManager.GetService<IEncryptionService>();
    var ioService = ServiceManager.GetService<IIOService>();

    long.TryParse(id, out var repoId);
    if (repoId == 0) {
      return;
    }

    var issues = await githubService.SelectRepository(_user, repoId);
    foreach (var issue in issues) {
      Console.WriteLine($"[{issue.Id}] {issue.Title}");
    }

    var backup = new BackupData();
    backup.RepositoryId = repoId;
    backup.Issues = issues;
    for (int i = 0; i < backup.Issues.Count; i++) {
      var wizardIssue = new WizardIssue();
      wizardIssue.Title = backup.Issues[i].Title;
      wizardIssue.Body = backup.Issues[i].Body;
      wizardIssue.Number = backup.Issues[i].Number;

      backup.WizardIssues.Add(wizardIssue);
    }

    var encrypted = encryptionService.EncryptData(backup, _user.EnryptionKey!);
    var filename = $"{repoId}-{DateTime.UtcNow.ToFileTime()}.wiz";

    ioService.SaveFile(encrypted, filename);
  }

  private void ReadFile(string filename) {
    var ioService = ServiceManager.GetService<IIOService>();
    var encryptionService = ServiceManager.GetService<IEncryptionService>();


    var file = ioService.LoadFile(filename);
    var data = encryptionService.DecryptData<BackupData>(file, _user.EnryptionKey!);
    Console.WriteLine(data.RepositoryId);
  }

  private void SetupServices() {
    var services = new ServiceCollection()
      .AddSingleton<IGitHubService, GitHubService>()
      .AddSingleton<IEncryptionService, EncryptionService>()
      .AddSingleton<IIOService, IOService>()
      .AddDbContext<WizardContext>()
      .AddScoped<DbUserRepository>()
      .AddScoped<IUserService, UserService>();

    ServiceManager.SetProvider(services);
  }
}

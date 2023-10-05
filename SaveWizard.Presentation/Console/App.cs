using SaveWizard.Models;
using SaveWizard.Authorization;
using SaveWizard.Authorization.GitHub;
using Microsoft.Extensions.DependencyInjection;
using SaveWizard.Core.Services;
using SaveWizard.Core.Interfaces;

namespace ConsoleWizard;

public class App {
  private WizardUser _user;
  public static IServiceProvider? Services { get; private set; }

  private string _option = "";

  public App() {
    _user = new WizardUser();
    _user.CreateGitHubUser("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");
  }

  public async Task<Task> Run() {
    Services = SetupServices();
    await Init();
    Render();
    return Task.CompletedTask;
  }

  private async Task<Task> Init() {
    var githubService = Services!.GetRequiredService<IGitHubService>();

    var userInfo = await githubService.GetBasicData(_user.UserData!.Client!);
    _user.Email = userInfo.Email;
    _user.Username = userInfo.Login;

    return Task.CompletedTask;
  }

  private void Render() {
    while (true) {
      ConsoleGraphics.Clear();
      ConsoleGraphics.DrawOptions();
      _option = Console.ReadLine();
      HandleOption();
    }
  }

  private void HandleOption() {
    switch (_option) {
      case WizardConstants.Print:
        Print();
        break;
      case WizardConstants.Select:
        Select();
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
    var githubService = Services!.GetRequiredService<IGitHubService>();

    var repos = await githubService.GetAllPrivateRepositories(_user);
    foreach (var repo in repos) {
      Console.WriteLine(repo.FullName);
    }
  }

  private void Select() {
    var githubService = Services!.GetRequiredService<IGitHubService>();
  }

  private IServiceProvider SetupServices() {
    var services = new ServiceCollection()
      .AddSingleton<IGitHubService, GitHubService>();

    return services.BuildServiceProvider();
  }
}

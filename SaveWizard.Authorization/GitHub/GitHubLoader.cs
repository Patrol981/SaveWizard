using Octokit;

using SaveWizard.Models;

namespace SaveWizard.Authorization.GitHub;
public static class GitHubLoader {
  public static void Authenticate(this WizardUser user) {
    user.Validate();
    user.UserData!.Client!.Credentials = new(user.UserData.AccessToken);
  }

  public static void CreateGitHubUser(this WizardUser user, string token) {
    user.UserData = new();
    user.UserData.AccessToken = token;
    user.UserData.Client = new(new ProductHeaderValue(WizardConstants.AppName));
    user.Authenticate();
  }
}

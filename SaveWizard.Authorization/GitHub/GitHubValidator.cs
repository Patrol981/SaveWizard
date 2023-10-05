using SaveWizard.Models;

namespace SaveWizard.Authorization.GitHub;
public static class GitHubValidator {
  public static void Validate(this WizardUser user) {
    if (user.UserData == null) {
      throw new NullReferenceException("UserData cannot be null");
    }
    if (user.UserData.Client == null) {
      throw new NullReferenceException("GitHub Client cannot be null");
    }
  }

  public static void EnsureCreated(this WizardUser user) {
    user.UserData ??= new();
  }
}

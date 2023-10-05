using SaveWizard.Models.Auth;

namespace SaveWizard.Models;
public class WizardUser {
  public string? Username { get; set; }
  public string? Email { get; set; }
  public GitHubUserData? UserData { get; set; }
}

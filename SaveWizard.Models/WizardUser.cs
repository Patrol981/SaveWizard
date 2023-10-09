using SaveWizard.Models.Auth;

namespace SaveWizard.Models;
public class WizardUser {
  public string? Username { get; set; }
  public string? Email { get; set; }
  public long PlatformId { get; set; }
  public Guid WizardId { get; set; }
  public string? EncryptionKey { get; set; }
  public GitHubUserData UserData { get; set; } = new();
}

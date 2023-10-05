using Octokit;

namespace SaveWizard.Models.Auth;
public class GitHubUserData {
  public string? AccessToken { get; set; }
  public GitHubClient? Client { get; set; }
}

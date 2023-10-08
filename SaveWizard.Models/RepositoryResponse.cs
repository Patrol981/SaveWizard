using Octokit;

namespace SaveWizard.Models;
public class RepositoryResponse {
  public string? RepositoryName { get; set; }
  public string? RepositoryFullName { get; set; }
  public long RepositoryId { get; set; }
  public List<WizardIssue> Issues { get; set; } = new();
}

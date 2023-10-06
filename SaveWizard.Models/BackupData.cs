using Octokit;

namespace SaveWizard.Models;
public class BackupData {
  public long RepositoryId { get; set; }
  public List<WizardIssue> WizardIssues { get; set; } = new();
  public List<Issue> Issues { get; set; } = new();
}

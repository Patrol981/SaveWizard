namespace SaveWizard.Models;
public class WizardIssue {
  public int Number { get; set; }
  public long Id { get; set; }
  public string? Title { get; set; }
  public string? Body { get; set; }
  public List<string> Labels { get; set; } = new();
  public List<string> Assignees { get; set; } = new();
}

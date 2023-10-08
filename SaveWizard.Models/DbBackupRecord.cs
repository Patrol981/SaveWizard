using System.ComponentModel.DataAnnotations;


namespace SaveWizard.Models;
public class DbBackupRecord {
  [Key]
  public Guid Id { get; set; }
  [Required]
  public Guid UserId { get; set; }
  [Required]
  public string? Filename { get; set; }
  [Required]
  public string? RepositoryName { get; set; }
  [Required]
  public DateTime Date { get; set; }
}

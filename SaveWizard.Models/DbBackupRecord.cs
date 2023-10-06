using System.ComponentModel.DataAnnotations;


namespace SaveWizard.Models;
public class DbBackupRecord {
  [Key]
  public Guid Id { get; set; }
  [Required]
  public int UserId { get; set; }
  [Required]
  public string? Filename { get; set; }
}

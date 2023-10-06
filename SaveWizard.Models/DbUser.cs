using System.ComponentModel.DataAnnotations;

namespace SaveWizard.Models;
public class DbUser {
  [Key]
  public Guid Id { get; set; }
  [Required]
  public long PlatformId { get; set; }
  public string? Name { get; set; }
  public string? Email { get; set; }
  [Required]
  public string? EnryptionKey { get; set; }
  [Required]
  public string? PersonalAccessToken { get; set; }
}

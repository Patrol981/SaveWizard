using SaveWizard.Models;

using Microsoft.EntityFrameworkCore;

namespace SaveWizard.Core;
public class WizardContext : DbContext {
  public DbSet<DbUser> Users { get; set; }
  public DbSet<DbBackupRecord> Backups { get; set; }

  public string DbPath { get; init; }

  public WizardContext() {
    var directory = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(directory);
    DbPath = Path.Join(path, "wizard.db");
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder.UseSqlite($"Data Source={DbPath}");
  }
}

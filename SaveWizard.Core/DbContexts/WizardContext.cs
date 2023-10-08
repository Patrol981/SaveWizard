using SaveWizard.Models;

using Microsoft.EntityFrameworkCore;
using SaveWizard.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SaveWizard.Core;
public class WizardContext : DbContext, IWizardDbContext {
  public DbSet<DbUser> Users { get; set; }
  public DbSet<DbBackupRecord> Backups { get; set; }

  public string DbPath { get; init; }

  public WizardContext() {
    var directory = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(directory);
    DbPath = Path.Join(path, "wizard.db");
  }

  public WizardContext(string dbPath) {
    DbPath = dbPath;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder.UseSqlite($"Data Source={DbPath}");
  }

  public void ConfigureContext(IServiceCollection services) {
    services.AddScoped<WizardContext>();
  }
}

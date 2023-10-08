using Microsoft.EntityFrameworkCore;

using SaveWizard.Core;

namespace SaveWizard.Tests;
public class WizardTestDbFixture {
  private const string ConnectionString = @"./wizardTest.db";
  private static readonly object _lock = new();
  private static bool _databaseInitialized;

  public WizardTestDbFixture() {
    lock (_lock) {
      if (!_databaseInitialized) {
        using (var context = CreateContext()) {
          context.Database.EnsureDeleted();
          context.Database.EnsureCreated();
          context.SaveChanges();
        }

        _databaseInitialized = true;
      }
    }
  }

  public WizardContext CreateContext() {
    var options = new DbContextOptionsBuilder<WizardContext>()
      .UseSqlite($"Data Source={ConnectionString}")
      .Options;

    var ctx = new WizardContext(ConnectionString);
    return ctx;
  }
}

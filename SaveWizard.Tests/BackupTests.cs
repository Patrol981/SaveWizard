using FluentAssertions;

using SaveWizard.Core.Repositories;
using SaveWizard.Models;

namespace SaveWizard.Tests;
public class BackupTests : IClassFixture<WizardTestDbFixture> {
  public WizardTestDbFixture Fixture { get; }

  public BackupTests(WizardTestDbFixture fixture)
    => Fixture = fixture;

  [Fact]
  public async void GetBackupById_ShouldReturnNotFound_WhenNoDataInDb() {
    using var ctx = Fixture.CreateContext();
    var backupRepo = new DbBackupRecordsRepository(ctx);

    var guid = Guid.NewGuid();

    var result = await backupRepo.GetRecordById(guid);

    result.Should().BeNull();
  }

  [Fact]
  public async void GetBackupsByUserId_ShouldReturnEmptyList_WhenNoDataInDb() {
    using var ctx = Fixture.CreateContext();
    var backupRepo = new DbBackupRecordsRepository(ctx);

    var guid = Guid.NewGuid();

    var result = await backupRepo.GetRecordsByUserId(guid);

    result.Count.Should().Be(0);
  }

  [Fact]
  public async void AddBackup_TaskShouldBeSuccess_WhenAddingNewRecord() {
    using var ctx = Fixture.CreateContext();
    var backupRepo = new DbBackupRecordsRepository(ctx);

    var backup = new DbBackupRecord() {
      Id = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      Filename = "SomeFileName",
      RepositoryName = "SomeRepoName",
      Date = DateTime.Now
    };

    var result = await backupRepo.AddRecord(backup);

    result.Should().NotBeNull();
    result.Should().BeOfType<DbBackupRecord>();
  }
}
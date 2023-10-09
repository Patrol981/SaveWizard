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
  public async void GetBackupsByUserIdOrdered_ShouldReturnEmptyList_WhenNoDataInDb() {
    using var ctx = Fixture.CreateContext();
    var backupRepo = new DbBackupRecordsRepository(ctx);

    var guid = Guid.NewGuid();

    var result = await backupRepo.GetRecordsByUserIdOrdered(guid);

    result.Count.Should().Be(0);
  }

  [Fact]
  public async void GetBackupById_ShouldReturnBackup_WhenDataProvided() {
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
    var backupInfo = await backupRepo.GetRecordById(backup.Id);

    backupInfo.Should().NotBeNull();
    backupInfo.Should().BeSameAs(backup);
  }

  [Fact]
  public async void GetBackupsByUserId_ShouldReturnList_WhenDataProvided() {
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
    var backupInfo = await backupRepo.GetRecordsByUserId(backup.UserId);

    backupInfo.Should().NotBeNull();
    backupInfo.Should().BeOfType<List<DbBackupRecord>>();
    backupInfo.Count.Should().Be(1);
  }

  [Fact]
  public async void GetBackupsByUserIdOrdered_ShouldReturnList_WhenDataProvided() {
    using var ctx = Fixture.CreateContext();
    var backupRepo = new DbBackupRecordsRepository(ctx);

    var userId = Guid.NewGuid();

    var backup = new DbBackupRecord() {
      Id = Guid.NewGuid(),
      UserId = userId,
      Filename = "SomeFileName",
      RepositoryName = "SomeRepoName",
      Date = DateTime.Now
    };

    var backup2 = new DbBackupRecord() {
      Id = Guid.NewGuid(),
      UserId = userId,
      Filename = "SomeFileName",
      RepositoryName = "SomeRepoName",
      Date = DateTime.Now.AddMinutes(5)
    };

    await backupRepo.AddRecord(backup);
    await backupRepo.AddRecord(backup2);
    var backupInfo = await backupRepo.GetRecordsByUserIdOrdered(userId);

    backupInfo.Should().NotBeNull();
    backupInfo.Should().BeOfType<List<IOrderedEnumerable<DbBackupRecord>>>();
    backupInfo.Count.Should().Be(1);
    foreach (var backupRecord in backupInfo) {
      backupRecord.Should().NotBeNull();
      backupRecord.Count().Should().Be(2);
    }
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

  [Fact]
  public async void RemoveBackup_TaskShouldBeSuccess_WhenRemovingNewRecord() {
    using var ctx = Fixture.CreateContext();
    var backupRepo = new DbBackupRecordsRepository(ctx);

    var backup = new DbBackupRecord() {
      Id = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      Filename = "SomeFileName",
      RepositoryName = "SomeRepoName",
      Date = DateTime.Now
    };

    await backupRepo.AddRecord(backup);
    var result = await backupRepo.RemoveRecord(backup.Id);

    result.Status.Should().Be(TaskStatus.RanToCompletion);
  }
}
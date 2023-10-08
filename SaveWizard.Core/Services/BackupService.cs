using Microsoft.Extensions.DependencyInjection;

using SaveWizard.Core.Interfaces;
using SaveWizard.Core.Repositories;
using SaveWizard.Models;

namespace SaveWizard.Core.Services;
public class BackupService : IBackupService {
  private readonly DbBackupRecordsRepository _recordsRepository;

  public BackupService() { }

  public BackupService(DbBackupRecordsRepository recordsRepository) {
    _recordsRepository = recordsRepository;
  }

  public async Task<Task> AddBackup(DbBackupRecord backup) {
    await _recordsRepository.AddRecord(backup);
    return Task.CompletedTask;
  }

  public async Task<Task> RemoveBackup(Guid id) {
    await _recordsRepository.RemoveRecord(id);
    return Task.CompletedTask;
  }

  public async Task<DbBackupRecord> GetBackupById(Guid id) {
    var backup = await _recordsRepository.GetRecordById(id);
    if (backup == null) {
      return null!;
    }
    return backup;
  }

  public async Task<List<DbBackupRecord>> GetBackupsByUserId(Guid id) {
    var backups = await _recordsRepository.GetRecordsByUserId(id);
    if (backups == null) {
      return null!;
    }
    return backups;
  }

  public async Task<List<DbBackupRecord>> GetLatestBackupsByUserId(Guid id) {
    var backups = await _recordsRepository.GetRecordsByUserIdOrdered(id);
    if (backups == null) {
      return null!;
    }
    var newest = new List<DbBackupRecord>();
    foreach (var backup in backups) {
      newest.Add(backup.First());
    }
    return newest;
  }

  public void DefineServices(IServiceCollection services) {
    services.AddScoped<DbBackupRecordsRepository>()
            .AddScoped<IBackupService, BackupService>();
  }
}

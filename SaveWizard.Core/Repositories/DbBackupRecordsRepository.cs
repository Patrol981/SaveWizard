using Microsoft.EntityFrameworkCore;

using SaveWizard.Models;

namespace SaveWizard.Core.Repositories;
public class DbBackupRecordsRepository {
  private readonly WizardContext _wizardContext;

  public DbBackupRecordsRepository(WizardContext wizardContext) {
    _wizardContext = wizardContext;
  }

  public async Task<DbBackupRecord> AddRecord(DbBackupRecord backupRecord) {
    await _wizardContext.Backups.AddAsync(backupRecord);
    await _wizardContext.SaveChangesAsync();
    return backupRecord;
  }

  public async Task<Task> RemoveRecord(Guid id) {
    var record = await _wizardContext.Backups.Where(x => x.Id == id).FirstAsync();
    if (record == null) {
      return Task.CompletedTask;
    }
    _wizardContext.Backups.Remove(record);
    await _wizardContext.SaveChangesAsync();
    return Task.CompletedTask;
  }

  public async Task<DbBackupRecord> GetRecordById(Guid id) {
    var record = await _wizardContext.Backups.FindAsync(id);
    if (record == null) {
      return null!;
    }
    return record;
  }

  public async Task<List<DbBackupRecord>> GetRecordsByUserId(Guid id) {
    var record = await _wizardContext.Backups.Where(x => x.UserId == id).ToListAsync();
    if (record == null) {
      return null!;
    }
    return record;
  }

  public async Task<List<IOrderedEnumerable<DbBackupRecord>>> GetRecordsByUserIdOrdered(Guid id) {
    var record = await _wizardContext.Backups.Where(x => x.UserId == id).ToListAsync();
    if (record == null) {
      return null!;
    }
    var result = record
      .GroupBy(x => x.RepositoryName)
      .Select(g => g.OrderByDescending(x => x.Date))
      .ToList();
    return result;
  }
}

using SaveWizard.Models;

namespace SaveWizard.Core.Interfaces;
public interface IBackupService : IWizardService {
  Task<Task> AddBackup(DbBackupRecord backup);
  Task<Task> RemoveBackup(Guid id);
  Task<DbBackupRecord> GetBackupById(Guid id);
  Task<List<DbBackupRecord>> GetBackupsByUserId(Guid id);
  Task<List<DbBackupRecord>> GetLatestBackupsByUserId(Guid id);
}

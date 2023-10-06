using SaveWizard.Models;

namespace SaveWizard.Core.Interfaces;
public interface IUserService {
  Task<WizardUser> GetUserById(Guid id);
  Task<WizardUser> GetUserByAccessToken(string token);
  Task<WizardUser> GetUserByPlatformId(long id);
  Task<Task> AddUser(WizardUser user);
}

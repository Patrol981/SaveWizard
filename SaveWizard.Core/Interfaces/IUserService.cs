using SaveWizard.Models;

namespace SaveWizard.Core.Interfaces;
public interface IUserService : IWizardService {
  Task<WizardUser> GetUserById(Guid id);
  Task<WizardUser> GetUserByAccessToken(string token);
  Task<WizardUser> GetUserByPlatformId(long id);
  Task<WizardUser> AddUser(WizardUser user);
}

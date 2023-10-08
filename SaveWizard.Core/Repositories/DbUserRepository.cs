using Microsoft.EntityFrameworkCore;

using SaveWizard.Models;

namespace SaveWizard.Core.Repositories;
public class DbUserRepository {
  private readonly WizardContext _wizardContext;

  public DbUserRepository(WizardContext userContext) {
    _wizardContext = userContext;
  }

  public async Task<DbUser> AddUser(DbUser user) {
    await _wizardContext.Users.AddAsync(user);
    await _wizardContext.SaveChangesAsync();
    return user;
  }

  public async Task<DbUser> GetUserById(Guid id) {
    var user = await _wizardContext.Users.FindAsync(id);
    if (user == null) {
      return null!;
    }
    return user;
  }

  public async Task<DbUser> GetUserByPlatformId(long id) {
    var user = await _wizardContext.Users.Where(x => x.PlatformId == id).FirstOrDefaultAsync();
    if (user == null) {
      return null!;
    }
    return user;
  }

  public async Task<DbUser> GetUserByAccessToken(string token) {
    var user = await _wizardContext.Users.Where(x => x.PersonalAccessToken == token).FirstOrDefaultAsync();
    if (user == null) {
      return null!;
    }
    return user;
  }
}

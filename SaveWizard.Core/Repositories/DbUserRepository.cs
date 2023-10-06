using Microsoft.EntityFrameworkCore;

using SaveWizard.Models;

namespace SaveWizard.Core.Repositories;
public class DbUserRepository {
  private readonly WizardContext _userContext;

  public DbUserRepository(WizardContext userContext) {
    _userContext = userContext;
  }

  public async Task<DbUser> AddUser(DbUser user) {
    await _userContext.Users.AddAsync(user);
    await _userContext.SaveChangesAsync();
    return user;
  }

  public async Task<DbUser> GetUserById(Guid id) {
    var user = await _userContext.Users.FindAsync(id);
    if (user == null) {
      return null!;
    }
    return user;
  }

  public async Task<DbUser> GetUserByPlatformId(long id) {
    var user = await _userContext.Users.Where(x => x.PlatformId == id).FirstOrDefaultAsync();
    if (user == null) {
      return null!;
    }
    return user;
  }

  public async Task<DbUser> GetUserByAccessToken(string token) {
    var user = await _userContext.Users.Where(x => x.PersonalAccessToken == token).FirstOrDefaultAsync();
    if (user == null) {
      return null!;
    }
    return user;
  }
}

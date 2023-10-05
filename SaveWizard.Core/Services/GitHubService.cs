using Octokit;

using SaveWizard.Core.Interfaces;
using SaveWizard.Models;

namespace SaveWizard.Core.Services;
public class GitHubService : IGitHubService {
  public async Task<User> GetBasicData(GitHubClient client) {
    var data = await client.User.Current();
    return data;
  }

  public async Task<List<Repository>> GetAllPrivateRepositories(WizardUser user) {
    var repos = await user.UserData!.Client!.Repository.GetAllForCurrent(new RepositoryRequest {
      Type = RepositoryType.Private
    });
    return repos.ToList();
  }

  public void SelectRepository() {

  }
}

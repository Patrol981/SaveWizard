using Octokit;

using SaveWizard.Models;

namespace SaveWizard.Core.Interfaces;
public interface IGitHubService : IWizardService {
  Task<User> GetBasicData(GitHubClient client);
  Task<List<Repository>> GetAllPrivateRepositories(WizardUser user);
  Task<RepositoryResponse> SelectRepository(WizardUser user, long repoId);
  Task<Task> AddIssues(WizardUser user, List<WizardIssue> issues, long repoId);
  Task<Task> AddIssue(WizardUser user, WizardIssue issue, long repoId);
}

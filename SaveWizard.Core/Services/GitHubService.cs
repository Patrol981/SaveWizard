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

  public async Task<List<Issue>> SelectRepository(WizardUser user, long repoId) {
    var issues = await user.UserData!.Client!.Issue.GetAllForRepository(repoId);
    return issues.ToList();
  }

  public async void AddIssues(WizardUser user, List<Issue> issues) {

  }

  public async void AddIssue(WizardUser user, Issue issue, long repoId) {
    var newIssue = new NewIssue(issue.Title);
    foreach (var asignee in issue.Assignees) {
      newIssue.Assignees.Add(asignee.Login);
    }
    foreach (var labels in issue.Labels) {
      newIssue.Labels.Add(labels.Name);
    }
    newIssue.Body = issue.Body;
    await user.UserData!.Client!.Issue.Create(repoId, newIssue);
  }
}

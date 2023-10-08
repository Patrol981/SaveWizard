using Microsoft.Extensions.DependencyInjection;

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

  public async Task<RepositoryResponse> SelectRepository(WizardUser user, long repoId) {
    var response = new RepositoryResponse();

    var issues = await user.UserData!.Client!.Issue.GetAllForRepository(repoId);
    var repoInfo = await user.UserData!.Client!.Repository.Get(repoId);

    foreach (var issue in issues) {
      var wizardIssue = new WizardIssue();
      wizardIssue.Title = issue.Title;
      wizardIssue.Body = issue.Body;
      wizardIssue.Number = issue.Number;
      wizardIssue.Id = issue.Id;

      foreach (var assignee in issue.Assignees) {
        wizardIssue.Assignees.Add(assignee.Login);
      }
      foreach (var label in issue.Labels) {
        wizardIssue.Labels.Add(label.Name);
      }

      response.Issues.Add(wizardIssue);
    }

    response.RepositoryName = repoInfo.Name;
    response.RepositoryFullName = repoInfo.FullName;
    response.RepositoryId = repoId;

    return response;
  }

  public async Task<Task> AddIssues(WizardUser user, List<WizardIssue> issues, long repoId) {
    for (short i = 0; i < issues.Count; i++) {
      await AddIssue(user, issues[i], repoId);
    }
    return Task.CompletedTask;
  }

  public async Task<Task> AddIssue(WizardUser user, WizardIssue issue, long repoId) {
    var newIssue = new NewIssue(issue.Title);
    foreach (var label in issue.Labels) {
      newIssue.Labels.Add(label);
    }
    foreach (var asignee in issue.Assignees) {
      newIssue.Assignees.Add(asignee);
    }
    foreach (var labels in issue.Labels) {
      newIssue.Labels.Add(labels);
    }
    newIssue.Body = issue.Body;
    await user.UserData!.Client!.Issue.Create(repoId, newIssue);
    return Task.CompletedTask;
  }

  public void DefineServices(IServiceCollection services) {
    services.AddScoped<IGitHubService, GitHubService>();
  }
}

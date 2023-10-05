using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;

using SaveWizard.Models;

namespace SaveWizard.Core.Interfaces;
public interface IGitHubService {
  Task<User> GetBasicData(GitHubClient client);
  Task<List<Repository>> GetAllPrivateRepositories(WizardUser user);
}

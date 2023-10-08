﻿using SaveWizard.Core.Interfaces;
using SaveWizard.Core.Repositories;
using SaveWizard.Models;
using SaveWizard.Authorization;
using SaveWizard.Authorization.GitHub;
using Microsoft.Extensions.DependencyInjection;

namespace SaveWizard.Core.Services;
public class UserService : IUserService {
  private readonly DbUserRepository _userRepository;

  public UserService() { }

  public UserService(DbUserRepository dbUserRepository) {
    _userRepository = dbUserRepository;
  }

  public async Task<WizardUser> AddUser(WizardUser user) {
    var newUser = new DbUser();
    newUser.Id = Guid.NewGuid();
    newUser.PlatformId = user.PlatformId;
    newUser.EnryptionKey = user.EnryptionKey;
    newUser.Email = user.Email;
    newUser.Name = user.Username;
    newUser.PersonalAccessToken = user.UserData!.AccessToken;

    await _userRepository.AddUser(newUser);

    user.WizardId = newUser.Id;

    return user;
  }

  public async Task<WizardUser> GetUserByAccessToken(string token) {
    var user = await _userRepository.GetUserByAccessToken(token);
    if (user == null) {
      return null!;
    }
    var wizard = new WizardUser();
    wizard.PlatformId = user.PlatformId;
    wizard.Username = user.Name;
    wizard.Email = user.Email;
    wizard.EnryptionKey = user.EnryptionKey;
    wizard.UserData.AccessToken = user.PersonalAccessToken;
    wizard.WizardId = user.Id;

    wizard.CreateGitHubUser(token);

    return wizard;
  }

  public Task<WizardUser> GetUserById(Guid id) {
    throw new NotImplementedException();
  }

  public Task<WizardUser> GetUserByPlatformId(long id) {
    throw new NotImplementedException();
  }

  public void DefineServices(IServiceCollection services) {
    services.AddScoped<DbUserRepository>();
    services.AddScoped<IUserService, UserService>();
  }
}

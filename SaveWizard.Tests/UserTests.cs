using FluentAssertions;

using SaveWizard.Core.Repositories;
using SaveWizard.Models;
namespace SaveWizard.Tests;
public class UserTests : IClassFixture<WizardTestDbFixture> {
  public WizardTestDbFixture Fixture { get; }

  public UserTests(WizardTestDbFixture fixture)
    => Fixture = fixture;

  [Fact]
  public async void GetUserById_ShouldReturnNull_WhenNoDataInDb() {
    using var ctx = Fixture.CreateContext();
    var userRepo = new DbUserRepository(ctx);

    var guid = Guid.NewGuid();
    var result = await userRepo.GetUserById(guid);

    result.Should().BeNull();
  }

  [Fact]
  public async void GetUserByPlatformId_ShouldRetrunNull_WhenNoDataInDb() {
    using var ctx = Fixture.CreateContext();
    var userRepo = new DbUserRepository(ctx);

    long id = 412412312312;
    var result = await userRepo.GetUserByPlatformId(id);

    result.Should().BeNull();
  }

  [Fact]
  public async void GetUserByAccessToken_ShouldReturnNull_WhenNoDataInDb() {
    using var ctx = Fixture.CreateContext();
    var userRepo = new DbUserRepository(ctx);

    string accessToken = "SomeRandomToken";
    var result = await userRepo.GetUserByAccessToken(accessToken);

    result.Should().BeNull();
  }

  [Fact]
  public async void GetUserById_ShouldReturnUser_WhenGivenData() {
    using var ctx = Fixture.CreateContext();
    var userRepo = new DbUserRepository(ctx);

    var dbUser = new DbUser() {
      Id = Guid.NewGuid(),
      PlatformId = long.MaxValue,
      Name = "Username",
      Email = "UserEmail@test.io",
      EncryptionKey = "AACCDDEEFFGG",
      PersonalAccessToken = "AABBCCDDEE"
    };
    await userRepo.AddUser(dbUser);

    var result = await userRepo.GetUserById(dbUser.Id);

    result.Should().NotBeNull();
    result.Should().BeOfType<DbUser>();
    result.Should().BeSameAs(dbUser);
  }

  [Fact]
  public async void GetUserByPlatformId_ShouldReturnUser_WhenGivenData() {
    using var ctx = Fixture.CreateContext();
    var userRepo = new DbUserRepository(ctx);

    var dbUser = new DbUser() {
      Id = Guid.NewGuid(),
      PlatformId = 3124134,
      Name = "Username",
      Email = "UserEmail@test.io",
      EncryptionKey = "AABBCCDDEEFFGG",
      PersonalAccessToken = "AABBDDEE"
    };
    await userRepo.AddUser(dbUser);

    var result = await userRepo.GetUserByPlatformId(dbUser.PlatformId);

    result.Should().NotBeNull();
    result.Should().BeOfType<DbUser>();
    result.Should().BeSameAs(dbUser);
  }

  [Fact]
  public async void GetUserByAccessToken_ShouldReturnUser_WhenGivenData() {
    using var ctx = Fixture.CreateContext();
    var userRepo = new DbUserRepository(ctx);

    var dbUser = new DbUser() {
      Id = Guid.NewGuid(),
      PlatformId = 4135143,
      Name = "Username",
      Email = "UserEmail@test.io",
      EncryptionKey = "AABBCCDD",
      PersonalAccessToken = "AABBDD"
    };
    await userRepo.AddUser(dbUser);

    var result = await userRepo.GetUserByAccessToken(dbUser.PersonalAccessToken);

    result.Should().NotBeNull();
    result.Should().BeOfType<DbUser>();
    result.Should().BeSameAs(dbUser);
  }

  [Fact]
  public async void AddUser_ShouldBeSuccess_WhenPerformed() {
    using var ctx = Fixture.CreateContext();
    var userRepo = new DbUserRepository(ctx);

    var dbUser = new DbUser() {
      Id = Guid.NewGuid(),
      PlatformId = 3141324123,
      Name = "Username",
      Email = "UserEmail@test.io",
      EncryptionKey = "AABBCCDDEEFFGG",
      PersonalAccessToken = "AABBCCDD"
    };
    var result = await userRepo.AddUser(dbUser);

    result.Should().NotBeNull();
    result.Should().BeOfType<DbUser>();
    result.Should().BeSameAs(dbUser);
  }
}

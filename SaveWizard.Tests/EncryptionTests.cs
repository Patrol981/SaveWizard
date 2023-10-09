using FluentAssertions;

using SaveWizard.Core.Services;
using SaveWizard.Models;

namespace SaveWizard.Tests;
public class EncryptionTests {
  private readonly EncryptionService _encryptionService;
  private readonly string _encryptionKey;

  public EncryptionTests() {
    _encryptionService = new EncryptionService();
    _encryptionKey = _encryptionService.GenerateKey();
  }

  [Fact]
  public void EncryptData_ShouldReturnEncrypted_WithDataGiven() {
    var dbUser = new DbUser() {
      Id = Guid.NewGuid(),
      PlatformId = 3141324123,
      Name = "Username",
      Email = "UserEmail@test.io",
      EncryptionKey = "AABBCCDDEEFFGG",
      PersonalAccessToken = "AABBCCDD"
    };

    var result = _encryptionService.EncryptData(dbUser, _encryptionKey);

    result.Should().NotBeNull();
    result.Should().BeOfType<byte[]>();
  }

  [Fact]
  public void DecryptData_ShouldRetrunDecrypted_WithDataGiven() {
    var dbUser = new DbUser() {
      Id = Guid.NewGuid(),
      PlatformId = 3141324123,
      Name = "Username",
      Email = "UserEmail@test.io",
      EncryptionKey = "AABBCCDDEEFFGG",
      PersonalAccessToken = "AABBCCDD"
    };

    var encrypted = _encryptionService.EncryptData(dbUser, _encryptionKey);
    var result = _encryptionService.DecryptData<DbUser>(encrypted, _encryptionKey);

    result.Should().NotBeNull();
    result.Should().BeOfType<DbUser>();
    result.Email.Should().Be(dbUser.Email);
    result.PlatformId.Should().Be(dbUser.PlatformId);
    result.Id.Should().Be(dbUser.Id);
    result.Name.Should().Be(dbUser.Name);
    result.EncryptionKey.Should().Be(dbUser.EncryptionKey);
    result.PersonalAccessToken.Should().Be(dbUser.PersonalAccessToken);
  }

  [Fact]
  public void DecrpytData_ShouldNotBeAbleToDecrypt_WhenGivenWrongKey() {
    var dbUser = new DbUser() {
      Id = Guid.NewGuid(),
      PlatformId = 3141324123,
      Name = "Username",
      Email = "UserEmail@test.io",
      EncryptionKey = "AABBCCDDEEFFGG",
      PersonalAccessToken = "AABBCCDD"
    };
    var key = _encryptionService.GenerateKey();

    var encrypted = _encryptionService.EncryptData(dbUser, _encryptionKey);
    Action result = () => _encryptionService.DecryptData<DbUser>(encrypted, key);

    result.Should().Throw<System.Security.Cryptography.CryptographicException>()
                   .WithMessage("The computed authentication tag did not match the input authentication tag.");
  }
}

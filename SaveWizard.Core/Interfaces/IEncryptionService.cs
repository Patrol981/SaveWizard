namespace SaveWizard.Core.Interfaces;
public interface IEncryptionService : IWizardService {
  byte[] EncryptData<T>(T data, string key);
  T DecryptData<T>(byte[] data, string key);
  string GenerateKey();
}

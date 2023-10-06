namespace SaveWizard.Core.Interfaces;
public interface IEncryptionService {
  byte[] EncryptData<T>(T data, string key);
  T DecryptData<T>(byte[] data, string key);
  string GenerateKey();
}

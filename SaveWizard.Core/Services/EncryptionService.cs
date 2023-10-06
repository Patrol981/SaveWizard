using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using SaveWizard.Core.Interfaces;

namespace SaveWizard.Core.Services;
public class EncryptionService : IEncryptionService {
  public byte[] EncryptData<T>(T data, string key) {
    var stringResult = DataToJsonString(data);
    var keyBytes = StringToByteArray(key);
    var encryptedData = AesGCMEncryption(stringResult, keyBytes);
    return encryptedData;
  }

  public T DecryptData<T>(byte[] data, string key) {
    var keyBytes = StringToByteArray(key);
    var decryptedData = AesGCMDecrypytion(data, keyBytes);
    var result = DeserializeJsonString<T>(decryptedData);
    return result;
  }

  public string GenerateKey() {
    int size = 256;
    using RandomNumberGenerator rng = RandomNumberGenerator.Create();
    byte[] key = new byte[size / 8];
    rng.GetBytes(key);

    return BitConverter.ToString(key).Replace("-", "");
  }

  private static string DataToJsonString<T>(T data) {
    return JsonSerializer.Serialize(data);
  }

  private static T DeserializeJsonString<T>(string data) {
    if (string.IsNullOrEmpty(data)) {
      throw new ArgumentNullException(nameof(data), "Input data cannot be null or empty");
    }
    var result = JsonSerializer.Deserialize<T>(data);
    if (result == null) {
      throw new ArgumentNullException(nameof(result), "Input data cannot be deserialized");
    }
    return result!;
  }

  private static byte[] StringToByteArray(string text) {
    int length = text.Length / 2;
    byte[] bytes = new byte[length];
    for (int i = 0; i < length; i++) {
      bytes[i] = Convert.ToByte(text.Substring(i * 2, 2), 16);
    }
    return bytes;
  }

  private static byte[] AesGCMEncryption(string data, byte[] key) {
    using AesGcm aesGcm = new AesGcm(key);

    var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
    RandomNumberGenerator.Fill(nonce);

    var dataBytes = Encoding.UTF8.GetBytes(data);
    var cipher = new byte[dataBytes.Length];
    var tag = new byte[AesGcm.TagByteSizes.MaxSize];

    aesGcm.Encrypt(nonce, dataBytes, cipher, tag);

    // tag and nonce are being save into file, key exists within user record in db
    var encryptedData = new byte[nonce.Length + tag.Length + cipher.Length];
    Buffer.BlockCopy(nonce, 0, encryptedData, 0, nonce.Length);
    Buffer.BlockCopy(tag, 0, encryptedData, nonce.Length, tag.Length);
    Buffer.BlockCopy(cipher, 0, encryptedData, nonce.Length + tag.Length, cipher.Length);

    return encryptedData;
  }
  private static string AesGCMDecrypytion(byte[] data, byte[] key) {
    using AesGcm aesGcm = new AesGcm(key);

    var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
    Buffer.BlockCopy(data, 0, nonce, 0, nonce.Length);

    var tag = new byte[AesGcm.TagByteSizes.MaxSize];
    Buffer.BlockCopy(data, nonce.Length, tag, 0, tag.Length);

    var cipher = new byte[data.Length - (nonce.Length + tag.Length)];
    Buffer.BlockCopy(data, nonce.Length + tag.Length, cipher, 0, cipher.Length);

    var decryptedBytes = new byte[cipher.Length];
    aesGcm.Decrypt(nonce, cipher, tag, decryptedBytes);

    var result = Encoding.UTF8.GetString(decryptedBytes);
    return result;
  }
}

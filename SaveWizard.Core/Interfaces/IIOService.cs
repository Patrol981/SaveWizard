namespace SaveWizard.Core.Interfaces;
public interface IIOService {
  void SaveFile(byte[] data, string filename);
  byte[] LoadFile(string filename);
}

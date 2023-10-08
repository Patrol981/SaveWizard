namespace SaveWizard.Core.Interfaces;
public interface IIOService : IWizardService {
  void SaveFile(byte[] data, string filename);
  byte[] LoadFile(string filename);
  ReadOnlySpan<string> GetBackups();
}

using Microsoft.Extensions.DependencyInjection;

using SaveWizard.Core.Interfaces;
using SaveWizard.Models;

namespace SaveWizard.Core.Services;
public class IOService : IIOService {
  public void SaveFile(byte[] data, string filename) {
    var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    var destination = Path.Combine(documentsPath, WizardConstants.SaveDirectory);
    Directory.CreateDirectory(destination);
    // no need for async there, since it is done on user's computer, it is faster than async call
    File.WriteAllBytes(Path.Combine(destination, filename), data);
  }

  public byte[] LoadFile(string filename) {
    var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    var destination = Path.Combine(documentsPath, WizardConstants.SaveDirectory, filename);
    // no need for async there, since it is done on user's computer, it is faster than async call
    return File.ReadAllBytes(destination);
  }

  public ReadOnlySpan<string> GetBackups() {
    var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    var destination = Path.Combine(documentsPath, WizardConstants.SaveDirectory);

    string[] files = Directory.GetFiles(destination);
    string[] filenames = new string[files.Length];

    for (short i = 0; i < files.Length; i++) {
      var filename = Path.GetFileName(files[i]);
      filenames[i] = filename;
    }

    return filenames;
  }

  public void DefineServices(IServiceCollection services) {
    services.AddSingleton<IIOService, IOService>();
  }
}

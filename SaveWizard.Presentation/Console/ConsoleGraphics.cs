using SaveWizard.Models;

namespace ConsoleWizard;
public static class ConsoleGraphics {
  public static void Clear() {
    Console.Clear();
  }

  public static void DrawOptions() {
    Console.WriteLine($"" +
      $"[{WizardConstants.Print} (repos/backups/all)] \n" +
      $"[{WizardConstants.Select} (repo/backup) (id)] \n" +
      $"[{WizardConstants.Delete} (backupId)] \n" +
      $"[{WizardConstants.Clear}]"
    );
  }
}

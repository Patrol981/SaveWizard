using SaveWizard.Models;

namespace ConsoleWizard;
public static class ConsoleGraphics {
  public static void Clear() {
    Console.Clear();
  }

  public static void DrawOptions() {
    Console.WriteLine($"" +
      $"[{WizardConstants.Print}]" +
      $"[{WizardConstants.Select} (repositoy)]" +
      $"[{WizardConstants.Clear}]"
    );
  }
}

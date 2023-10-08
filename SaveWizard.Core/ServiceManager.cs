
using Microsoft.Extensions.DependencyInjection;

using SaveWizard.Core.Interfaces;

namespace SaveWizard.Core;
public static class ServiceManager {
  public static IServiceProvider? Provider { get; private set; }
  public static IServiceCollection Services { get; private set; } = new ServiceCollection();

  public static void SetProvider(IServiceCollection serviceDescriptors) {
    Provider = serviceDescriptors.BuildServiceProvider();
  }

  public static T GetService<T>() => Provider.GetRequiredService<T>();

  public static void ConfigureServices(this IServiceCollection services, params Type[] markers) {
    var wizardServices = new List<IWizardService>();
    foreach (var marker in markers) {
      wizardServices.AddRange(marker.Assembly.ExportedTypes
        .Where(x => typeof(IWizardService).IsAssignableFrom(x) && !x.IsAbstract)
        .Select(Activator.CreateInstance).Cast<IWizardService>());
    }

    foreach (var wizardService in wizardServices) {
      wizardService.DefineServices(services);
    }

    services.AddSingleton(wizardServices as IReadOnlyCollection<IWizardService>);
    SetProvider(services);
  }
}


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

  public static void ConfigureAll(this IServiceCollection services, params Type[] markers) {
    var wizardServices = new List<IWizardService>();
    var contexts = new List<IWizardDbContext>();

    foreach (var marker in markers) {
      wizardServices.AddRange(GetAssemblies<IWizardService>(marker));
      contexts.AddRange(GetAssemblies<IWizardDbContext>(marker));
    }

    foreach (var wizardService in wizardServices) {
      wizardService.DefineServices(services);
    }

    foreach (var context in contexts) {
      context.ConfigureContext(services);
    }

    services.AddSingleton(wizardServices as IReadOnlyCollection<IWizardService>);
    services.AddSingleton(contexts as IReadOnlyCollection<IWizardDbContext>);
  }

  public static void ConfigureServices(this IServiceCollection services, params Type[] markers) {
    var wizardServices = new List<IWizardService>();

    foreach (var marker in markers) {
      wizardServices.AddRange(GetAssemblies<IWizardService>(marker));
    }

    foreach (var wizardService in wizardServices) {
      wizardService.DefineServices(services);
    }

    services.AddSingleton(wizardServices as IReadOnlyCollection<IWizardService>);
  }

  private static IEnumerable<T> GetAssemblies<T>(Type marker) {
    return marker.Assembly.ExportedTypes
        .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsAbstract)
        .Select(Activator.CreateInstance).Cast<T>();
  }
}


using Microsoft.Extensions.DependencyInjection;

namespace KizerSharp.Services;
public sealed class ServiceManager {
  public static IServiceProvider? Provider { get; private set; }

  public static void SetProvider(IServiceCollection serviceDescriptors) {
    Provider = serviceDescriptors.BuildServiceProvider();
  }

  public static T GetService<T>() => Provider.GetRequiredService<T>();
}

using Microsoft.Extensions.DependencyInjection;

namespace SaveWizard.Core.Interfaces;
public interface IWizardService {
  void DefineServices(IServiceCollection services);
}

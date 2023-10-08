using Microsoft.Extensions.DependencyInjection;

namespace SaveWizard.Core.Interfaces;
public interface IWizardDbContext {
  void ConfigureContext(IServiceCollection services);
}

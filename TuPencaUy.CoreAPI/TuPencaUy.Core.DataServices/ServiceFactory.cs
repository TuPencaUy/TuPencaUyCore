using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DataServices.Services.CommonLogic;
using TuPencaUy.Core.DataServices.Services.Platform;
using TuPencaUy.Platform.DAO.Models.Data;
using TuPencaUy.Site.DAO.Models.Data;

namespace TuPencaUy.Core.DataServices
{
  public class ServiceFactory : IServiceFactory
  {
    private IServiceCollection _serviceCollection;
    private readonly IConfiguration _configuration;

    public ServiceFactory(IConfiguration configuration)
    {
      _configuration = configuration;
      CreateBasicPlatformServices();
    }

    public TService GetService<TService>()
    {
      return _serviceCollection.BuildServiceProvider().GetRequiredService<TService>(); 
    }

    private void CommonInjections()
    {
      _serviceCollection.AddScoped<IAuthLogic, AuthLogic>();
    }

    private void CreateBasicPlatformServices()
    {
      _serviceCollection = new ServiceCollection();
      _serviceCollection.AddDbContext<PlatformDbContext>(options =>
      {
        options.UseSqlServer(_configuration.GetConnectionString("Platform"))
        .LogTo(s => System.Diagnostics.Debug.WriteLine(s)); // To log queries
      });

      _serviceCollection.AddScoped<IConfiguration>(_ => _configuration);
      CommonInjections();

      _serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(PlatformGenericRepository<>));
      _serviceCollection.AddScoped<ISiteService, PlatformSiteService>();
    }

    public void CreatePlatformServices()
    {
      _serviceCollection = new ServiceCollection();
      _serviceCollection.AddDbContext<PlatformDbContext>(options =>
      {
        options.UseSqlServer(_configuration.GetConnectionString("Platform"))
        .LogTo(s => System.Diagnostics.Debug.WriteLine(s)); // To log queries
      });

      _serviceCollection.AddScoped<IConfiguration>(_ => _configuration);
      CommonInjections();

      _serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(PlatformGenericRepository<>));
      _serviceCollection.AddScoped<IAuthService, PlatformAuthService>();
      _serviceCollection.AddScoped<IUserService, PlatformUserService>();
      _serviceCollection.AddScoped<ISiteService, PlatformSiteService>();
    }

    public void CreateTenantServices(string connectionString)
    {
      CommonInjections();

      _serviceCollection = new ServiceCollection();
      _serviceCollection.AddDbContext<SiteDbContext>(options =>
      {
        options.UseSqlServer(connectionString)
        .LogTo(s => System.Diagnostics.Debug.WriteLine(s)); // To log queries
      });

      _serviceCollection.AddScoped<IConfiguration>(_ => _configuration);
      CommonInjections();

      _serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(SiteGenericRepository<>));
      _serviceCollection.AddScoped<IAuthService, SiteAuthService>();
      _serviceCollection.AddScoped<IUserService, SiteUserService>();
    }
  }
}

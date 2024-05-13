using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Platform.DAO.Models.Data;
using TuPencaUy.Platform.DataServices.Services;
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

    private void CreateBasicPlatformServices()
    {
      _serviceCollection = new ServiceCollection();
      _serviceCollection.AddDbContext<PlatformDbContext>(options =>
      {
        options.UseSqlServer(_configuration.GetConnectionString("Platform"))
        .LogTo(s => System.Diagnostics.Debug.WriteLine(s)); // To log queries
      });
      _serviceCollection.AddScoped<IConfiguration>(_ => _configuration);
      _serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(PlatformGenericRepository<>));
      _serviceCollection.AddScoped<ISiteService, SiteService>();
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
      _serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(PlatformGenericRepository<>));
      _serviceCollection.AddScoped<IAuthService, AuthService>();
      _serviceCollection.AddScoped<IUserService, UserService>();
      _serviceCollection.AddScoped<ISiteService, SiteService>();
    }

    public void CreateTenantServices(string connectionString)
    {
      _serviceCollection = new ServiceCollection();
      _serviceCollection.AddDbContext<SiteDbContext>(options =>
      {
        options.UseSqlServer(_configuration.GetConnectionString(connectionString))
        .LogTo(s => System.Diagnostics.Debug.WriteLine(s)); // To log queries
      });
      _serviceCollection.AddScoped<IConfiguration>(_ => _configuration);
      _serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(SiteGenericRepository<>));
      _serviceCollection.AddScoped<IAuthService, AuthService>();
      _serviceCollection.AddScoped<IUserService, UserService>();
    }
  }
}

namespace TuPencaUy.Core.DataServices
{
  public interface IServiceFactory
  {
    T GetService<T>();
    void CreatePlatformServices();
    void CreateTenantServices(string connectionString);
  }
}

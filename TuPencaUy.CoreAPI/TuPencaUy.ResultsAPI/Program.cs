using Microsoft.EntityFrameworkCore;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DataServices.Services.Platform;
using TuPencaUy.Platform.DAO.Models.Data;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Platform");

// Creates platform db if not exists
var options = new DbContextOptionsBuilder<PlatformDbContext>()
      .UseSqlServer(connString)
      .Options;
var dbContext = new PlatformDbContext(options);
dbContext.Database.Migrate();
dbContext.Dispose();

// Add services to the container.
builder.Services.AddDbContext<PlatformDbContext>(options =>
{
  options
  .UseLazyLoadingProxies()
  .UseSqlServer(connString)
  .LogTo(s => System.Diagnostics.Debug.WriteLine(s)); // To log queries
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(PlatformGenericRepository<>));
builder.Services.AddScoped<IEventService, PlatformEventService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
  );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

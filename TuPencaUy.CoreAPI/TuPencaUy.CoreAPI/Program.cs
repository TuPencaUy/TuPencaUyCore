using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TuPencaUy.Core.API.Middlewares;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services.CommonLogic;
using TuPencaUy.Platform.DAO.Models.Data;
using TuPencaUy.Site.DAO.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
builder.Services.AddScoped<IAuthLogic, AuthLogic>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.RequireHttpsMetadata = false;
      options.SaveToken = true;
      options.TokenValidationParameters = new TokenValidationParameters()
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidIssuer = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
      };
    });


//To run migrations
builder.Services.AddDbContext<SiteDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("Platform"))
  .LogTo(s => System.Diagnostics.Debug.WriteLine(s)); // To log queries
});


// Creates platform db if not exists
var options = new DbContextOptionsBuilder<PlatformDbContext>()
      .UseSqlServer(builder.Configuration.GetConnectionString("Platform"))
      .Options;
var dbContext = new PlatformDbContext(options);

dbContext.Database.Migrate();

dbContext.Dispose();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<RequestContentMiddleware>();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(builder => builder
  .AllowAnyOrigin()
  .AllowAnyMethod()
  .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

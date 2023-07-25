using PA.Reflect.Daily.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PA.Reflect.Daily.Core.Interfaces;
using MediatR;
using PA.Reflect.Daily.Core;

namespace PA.Reflect.Daily.Infrastructure;

public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection services, string connectionString) {
      services.AddDbContextFactory<AppDbContext>(options =>
        options.UseSqlServer(connectionString)); // will be created in web project root
    }
}

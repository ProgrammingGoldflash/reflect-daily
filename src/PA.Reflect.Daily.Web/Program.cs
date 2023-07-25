using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PA.Reflect.Daily.Infrastructure;
using PA.Reflect.Daily.Infrastructure.Data;
using Autofac;
using PA.Reflect.Daily.Core;
using Autofac.Extensions.DependencyInjection;
using PA.Reflect.Daily.Web.Interfaces;
using PA.Reflect.Daily.Web.Services;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Bootstrap;
using PA.Reflect.Daily.ServiceLayer.Interfaces;
using PA.Reflect.Daily.ServiceLayer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");  // builder.Configuration.GetConnectionString("SqliteConnection");

builder.Services.AddDbContext(connectionString!);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IReflectionService, ReflectionService>();

builder.Services
  .AddBlazorise()
  .AddBootstrapProviders()
  .AddFontAwesomeIcons();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
  options.Domain = "";
  options.ClientId = "";
  options.ClientSecret = "";
  options.ResponseType = "code";
});

builder.Services.AddHttpContextAccessor();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions
{
  MinimumSameSitePolicy = SameSiteMode.None,
  Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCookiePolicy();

app.MapDefaultControllerRoute();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

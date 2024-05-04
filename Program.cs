using PortfolioApp.Components;
using Microsoft.AspNetCore.Identity;
using PortfolioApp.Models;
using System;
using PortfolioApp;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Services.Interfaces;
using PortfolioApp.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerSideBlazor(options =>
{
	options.DetailedErrors = true;
});

builder.Services.AddOptions();
var _AppSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntiforgery(o => o.HeaderName = "X-CSRF-TOKEN");
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddScoped<IDbHelper, DbHelper>();
builder.Services.AddScoped<IAssetGetter, AssetGetter>();
builder.Services.AddScoped<IUserGetter, UserGetter>();
builder.Services.AddScoped<ITransactionGetter, TransactionGetter>();
builder.Services.AddScoped<IDictGetter,DictGetter>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BaseUrl"]) });

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(_AppSettings.ConnectionString),
	ServiceLifetime.Scoped);
builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 2;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AppDbContext>();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();
app.MapControllerRoute(
	   name: "default",
	   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

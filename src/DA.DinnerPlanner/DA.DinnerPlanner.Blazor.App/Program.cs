using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.GeoCode;
using EFCore.DbContextFactory.Extensions;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// init Hangfire stuff
builder.Services.AddHangfire(cfg => cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
	.UseInMemoryStorage()
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
);
builder.Services.AddHangfireServer();

// add local appsettings
builder.Configuration.AddJsonFile("appsettings.local.json", optional: false);    // there is the connstring which will not be committed to 
GlobalConfiguration.Configuration.UseColouredConsoleLogProvider();
string connString = builder.Configuration.GetConnectionString("da_dinnerplanner-db")!;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<DinnerPlannerContext>(builder => builder.UseMySQL(connString));
builder.Services.AddSingleton<IDbContextFactory<DinnerPlannerContext>, DinnerPlannerContextFactory>();
builder.Services.AddSingleton<IGeoCoder, OsmGeoCoder>();    // TODO DA: get this info from Cfg
builder.Services.AddQuickGridEntityFrameworkAdapter();

// https://developers.facebook.com/apps/
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-8.0

//builder.Services.AddAuthentication().AddFacebook(opts =>
//{
//	opts.AppId = "1619827918716401";
//	opts.AppSecret = "ad93dba26b8231105261b22df749a06b";
//	opts.AccessDeniedPath = "/IDPAccessDenied";
//	opts.Events.OnCreatingTicket = context =>
//	{
//		return Task.CompletedTask;
//	};
//});

var app = builder.Build();
// https://www.endycahyono.com/article/aspnetcore3-running-under-subdirectory-on-nginx
string? pathBase = builder.Configuration["webPathBase"];
if (!string.IsNullOrEmpty(pathBase))
	app.UsePathBase(pathBase);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseHangfireDashboard();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

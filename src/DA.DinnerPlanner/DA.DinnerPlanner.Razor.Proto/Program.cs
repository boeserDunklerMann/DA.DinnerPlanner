using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using Google.Apis.Auth.AspNetCore3;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DA.DinnerPlanner.Razor.Proto
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			// init Hangfire stuff
			builder.Services.AddHangfire(cfg => cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
			.UseInMemoryStorage()
			.UseSimpleAssemblyNameTypeSerializer()
			);
			builder.Services.AddHangfireServer();
			

			// Add services to the container.
			builder.Services.AddRazorPages()/*.WithRazorPagesRoot("/dinnerplan")*/;
			builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);    // there is the connstring which will not be committed to git
			CreateDIBindings(builder);

			// This configures Google.Apis.Auth.AspNetCore3 for use in this app.
			builder.Services.AddAuthentication(o =>
			{
				// This forces challenge results to be handled by Google OpenID Handler, so there's no
				// need to add an AccountController that emits challenges for Login.
				o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
				// This forces forbid results to be handled by Google OpenID Handler, which checks if
				// extra scopes are required and does automatic incremental auth.
				o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
				// Default scheme that will handle everything else.
				// Once a user is authenticated, the OAuth2 token info is stored in cookies.
				o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

			}).AddCookie();

			var app = builder.Build();

			// https://www.endycahyono.com/article/aspnetcore3-running-under-subdirectory-on-nginx
			string? pathBase = builder.Configuration["webPathBase"];
			if (!string.IsNullOrEmpty(pathBase))
				app.UsePathBase(pathBase);

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
			}
			app.UseStaticFiles();

			app.UseHangfireDashboard();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
		private static void CreateDIBindings(WebApplicationBuilder? builder)
		{
			if (builder == null)
				throw new NullReferenceException(nameof(builder));
			builder.Services.AddScoped<IDinnerPlannerContext, DinnerPlannerContext>();
		}

	}
}

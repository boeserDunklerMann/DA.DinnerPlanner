using DA.DinnerPlanner.Web.Data;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Web
{
	/// <ChangeLog>
	/// <Create Datum="29.01.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddRazorPages();
			IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
			builder.Services.AddAuthentication(opts =>
			{
				// This forces challenge results to be handled by Google OpenID Handler, so there's no
				// need to add an AccountController that emits challenges for Login.
				opts.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
				// This forces forbid results to be handled by Google OpenID Handler, which checks if
				// extra scopes are required and does automatic incremental auth.
				opts.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
				// Default scheme that will handle everything else.
				// Once a user is authenticated, the OAuth2 token info is stored in cookies.
				opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
				.AddCookie()
				.AddGoogleOpenIdConnect(options =>
			{
				options.ClientId = googleAuthNSection["ClientID"];
				options.ClientSecret = googleAuthNSection["ClientSecret"];
			});
			IConfigurationSection facebookAuthSection = builder.Configuration.GetSection("Authentication:FB");
			builder.Services.AddAuthentication().AddFacebook(opts =>
			{
				opts.AppId = facebookAuthSection["AppID"]!;
				opts.AppSecret = facebookAuthSection["AppSecret"]!;
			});
			
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}
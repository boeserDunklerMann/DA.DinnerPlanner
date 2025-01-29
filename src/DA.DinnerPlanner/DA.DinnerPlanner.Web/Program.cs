using DA.DinnerPlanner.Web.Data;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
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
			builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);    // there are some secrets which will not be committed to git
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddRazorPages();
			IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
			IConfigurationSection facebookAuthSection = builder.Configuration.GetSection("Authentication:FB");
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
				options.AccessDeniedPath = "/IDPAccessDenied";
			})
				.AddFacebook(opts =>
			{
				opts.AppId = facebookAuthSection["AppID"]!;
				opts.AppSecret = facebookAuthSection["AppSecret"]!;
				opts.AccessDeniedPath = "/IDPAccessDenied";
				
				opts.Events.OnCreatingTicket = ctx =>
				{
					List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();
					tokens.Add(new AuthenticationToken()
					{
						Name = "TicketCreated",
						Value = DateTime.UtcNow.ToString()
					});
					ctx.Properties.StoreTokens(tokens);
					return Task.CompletedTask;
				};
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
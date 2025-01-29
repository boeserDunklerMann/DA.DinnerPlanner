# DA.DinnerPlanner
Web-based planning tool for serving a dinner, for randomly chosen people (like the german TV-show "Das perfekte Dinner"), inspired by [this](https://www.giessenkocht.de/) project, I decided to write my own
webportal of this type. Just for fun.

# DA.DinnerPlanner.Model
Datamodel with databasecontext
## TODO
see [Jira Board](https://boeserdunklermann.atlassian.net/jira/software/projects/DPLAN/boards/4)

# DA.DinnerPlanner.EFCore.Setup.Cons
Console project for setting up the database and generating some testdata

# DA.DinnerPlanner.Razor.Proto
Webapp-prototype with DB-access via EFCore

## External IDP How-To

### Google
1. Go to [Google API-Console](https://console.cloud.google.com/auth/clients?inv=1&invt=AboIKg)
2. Create a project first!
3. Add OAuth 2.0 Client ID copy&paste Client-ID and -credentials
4. in section **Authorized redirect URIs** add `https://localhost:{PORT}/signin-google`
see also [here](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0#create-the-google-oauth-20-client-id-and-secret)
5. create WebApp in VS
5.1. add package `Google.Apis.Auth.AspNetCore3`
5.2. in `program.cs` add following:
			```
   
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

			app.UseAuthentication();
			app.UseAuthorization();
```

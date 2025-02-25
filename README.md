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
**Debug this with IIS Express because of HTTPS**

**WebApp with "Individual User Accounts"**
### Google
1. Go to [Google API-Console](https://console.cloud.google.com/auth/clients?inv=1&invt=AboIKg)
2. Create a project first!
3. Add OAuth 2.0 Client ID copy&paste Client-ID and -credentials
4. in section **Authorized redirect URIs** add `https://localhost:{PORT}/signin-google`

see also [here](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0#create-the-google-oauth-20-client-id-and-secret)

5. create WebApp in VS
	5.1. add package `Google.Apis.Auth.AspNetCore3`
	5.2. in `program.cs` add following:

~~~
   
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
~~~
### Facebook
see also [here](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-8.0#create-the-app-in-facebook)

1. Add package `Microsoft.AspNetCore.Authentication.Facebook`
2. Go to [Facebook Developers page](https://developers.facebook.com/) and create developer account
3. create a new App
3.1. fill **Client OAuth settings** page
3.2. use defaults, fill **redirect URIs**: `https://localhost:{Port}/signin-facebook`
4. Linke Navigation: Settings->Allgemein
4.1. App-ID und App-Secrets merken (vorerst in appsettings.json speichern)
4.2. Rest ausfüllen

#### Configure WwebApp for FB Auth

```

services.AddAuthentication().AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
        facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
    });
```

# Docker support
## Install on RaspiOS
### Add docker's official GPG key
Run following command as root:

```
 apt-get install ca-certificates curl
 install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/raspbian/gpg -o /etc/apt/keyrings/docker.asc
chmod a+r /etc/apt/keyrings/docker.asc
```
### Add the repository to Apt sources
Run following command as root:

```
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/raspbian \
  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

apt-get update
```

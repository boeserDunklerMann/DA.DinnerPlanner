namespace DA.DinnerPlanner.Razor.Proto
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages()/*.WithRazorPagesRoot("/dinnerplan")*/;
			builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);    // there is the connstring which will not be committed to git

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

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}

using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.GeoCode;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: false);    // there is the connstring which will not be committed to git

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IDinnerPlannerContext, DinnerPlannerContext>();   // TODO DA: get this info from Cfg
builder.Services.AddSingleton<IGeoCoder, OsmGeoCoder>();	// TODO DA: get this info from Cfg

var app = builder.Build();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

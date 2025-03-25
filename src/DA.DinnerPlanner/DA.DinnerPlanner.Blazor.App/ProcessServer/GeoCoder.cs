using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DA.DinnerPlanner.Blazor.App.ProcessServer
{
	/// <ChangeLog>
	/// <Create Datum="25.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class GeoCoder : IGeoCoder
	{
		private IDinnerPlannerContext? context;
		public async Task ProcessAllUsersAsync(IDinnerPlannerContext? ctx, string? ConnectionString, Model.Contracts.IGeoCoder geo)
		{
			Debug.WriteLine($"Hello from {nameof(ProcessAllUsersAsync)}");
			if (ctx == null)
			{
				context = new DinnerPlannerContext();
				context.ConnectionString = ConnectionString!;
			}
			else
				context = ctx;
			// gib mir alle user, die min. eine geoloc mit null haben ODER wo die geoloc.result != ok ist
			var usersWOLocation = (from user in context.Users
								   where user.AddressList.Count(al => al.GeoLocation == null || al.GeoLocation.GeoCodeResult != Model.GeoCode.GeoCodeResult.OK) >= 1
								   select user).Take(10);

			usersWOLocation.ToList().ForEach(async u => 
			{
				Address? primary;
					primary = u.AddressList.FirstOrDefault(a => a.Primary);
				if (primary == null)
				{
					// user has no primary address, so we take the first one
					primary = u.AddressList.First();
				}
				var geoLocation = await geo.Address2LocationAsync(primary);
				if (primary.GeoLocation == null)
					primary.GeoLocation = geoLocation;
				else
				{
					primary.GeoLocation.Longitude = geoLocation.Longitude;
					primary.GeoLocation.Latitude = geoLocation.Latitude;
					primary.GeoLocation.GeoCodeResult = geoLocation.GeoCodeResult;
				}
				Task.Delay(1500).Wait();
			});
			await context.SaveAsync();
			await Task.CompletedTask;
		}
	}
}
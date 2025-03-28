using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;

namespace DA.DinnerPlanner.Blazor.App.ProcessServer
{
	/// <ChangeLog>
	/// <Create Datum="25.03.2025" Entwickler="DA" />
	/// <Change Datum="26.03.2025" Entwickler="DA">Context removed from ProcessAllUsersAsync, because of DBContextFactory (Jira-Nr. DPLAN-80)</Change>
	/// <Change Datum="28.03.2025" Entwickler="DA">use primary c'tor</Change>
		/// </ChangeLog>
	public class GeoCoderProcess(Func<DinnerPlannerContext> factory) : IGeoCoderProcess
	{
		public async Task ProcessAllUsersAsync(string? ConnectionString, IGeoCoder geo)
		{
			// When running as a Hangfire-Job, we need to create a new DB-Context
			using (var context = factory.Invoke())
			{
				context.ConnectionString = ConnectionString!;

				// gib mir alle user, die min. eine geoloc mit null haben ODER wo die geoloc.result != ok ist
				var usersWOLocation = (from user in context.Users
									   where user.AddressList.Count(al => al.GeoLocation == null || al.GeoLocation.GeoCodeResult != Model.GeoCode.GeoCodeResult.OK) >= 1
									   select user);

				usersWOLocation.ToList().ForEach(async u =>
				{
					Address? primary;
					primary = u.AddressList.FirstOrDefault(a => a.Primary);
					//user has no primary address, so we take the first one
					primary ??= u.AddressList.First();
					//if (primary == null)	// this is the same as the compound line above
					//{
					//	primary = u.AddressList.First();
					//}
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
			}
		}
	}
}
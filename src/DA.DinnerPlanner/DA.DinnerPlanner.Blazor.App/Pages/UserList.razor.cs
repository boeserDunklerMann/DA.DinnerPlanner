using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public partial class UserList : ComponentBase
	{
		private IQueryable<User>? Users { get; set; }
		private User NewUser { get; set; } = new();
		private DinnerPlannerContext? dpcontext;
		/// <summary>
		/// Identifies whether a db-action is currently in progress
		/// </summary>
		private bool Loading { get; set; } = false;

		protected override async Task OnInitializedAsync()
		{
			if (dpcontext == null)
			{
				dpcontext = await contextFactory.CreateDbContextAsync();
				dpcontext.ConnectionString = cfg.GetConnectionString("da_dinnerplanner-db")!;
			}

			if (Loading)
				return;
			try
			{
				Loading = true;
				Users = (await Application.Instance.GetAllUsersAsync(dpcontext)).AsQueryable();
			}
			finally
			{
				Loading = false;
			}
			await base.OnInitializedAsync();
		}

		private async Task NewUserSubmittedAsync()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				await Application.Instance.CreateUserAsync(dpcontext!, NewUser);
				//Users = await Application.Instance.GetAllUsersAsync(dpcontext);
				navMgr.NavigateTo(nameof(UserList), true);
			}
			finally
			{
				Loading = false;
			}
		}

		private async Task OnGeoCodeAsync(User user)
		{
			if (Loading)
				return;
			try
			{
				Address? usersPrimaryAddress = null;
				usersPrimaryAddress = user.AddressList.FirstOrDefault(a => a.Primary);
				// user has no primary address, so we take the first one
				usersPrimaryAddress ??= user.AddressList.FirstOrDefault();
				if (usersPrimaryAddress != null)
				{
					usersPrimaryAddress.GeoLocation = await geo.Address2LocationAsync(usersPrimaryAddress);
					await dpcontext!.SaveAsync();
				}
			}
			finally
			{
				Loading = false;
			}
		}
		#region Disposing
		// see: https://learn.microsoft.com/de-de/dotnet/fundamentals/code-analysis/quality-rules/ca1816#example-that-satisfies-ca1816

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				dpcontext?.Dispose();
				dpcontext = null;
			}
		}
		#endregion
	}
}
using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="17.03.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public partial class EditAddresses : ComponentBase
	{
		[Parameter]
		public int UserID { get; set; }
		private User? EditingUser { get; set; }
		private readonly Application application = Application.Instance;
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
			if ( Loading)
			{
				return;
			}
			try
			{
				Loading = true;
				if (UserID > 0)
				{
					EditingUser = await application.GetUserByIdAsync(dpcontext, UserID);
				}
			}
			finally
			{
				Loading = false;
			}
		}

		private async Task OnAddressDeleteAsync(Address	address2delete)
		{
			if (Loading)
				return;
			try
			{
				Loading=true;
				address2delete.Delete();
				await dpcontext!.SaveAsync();
			}
			finally
			{
				Loading = false;
			}
		}
		private async Task OnSaveAsync()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				await dpcontext!.SaveAsync();
			}
			finally
			{
				Loading = false;
			}
		}

		private async Task AddAddressAsync()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				if (UserID > 0)
				{
					EditingUser!.AddressList.Add(new());
					await dpcontext!.SaveAsync();
				}
			}
			finally
			{
				Loading = false;
			}
		}

		private async Task PrimaryAddressChanged(Address changedAddress)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				if (UserID > 0)
				{
					bool newValue = !changedAddress.Primary;    // the bound value is not yet updated
					if (newValue)   // if this is our new primary address, all others must not be primary
					{
						foreach (Address item in EditingUser!.AddressList)
						{
							if (item.Id != changedAddress.Id)
								item.Primary = false;
						}
						changedAddress.Primary = newValue;  // update value because binding is not yet done, we need this for saving
						await dpcontext!.SaveAsync();
					}
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
using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="17.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class EditAddresses : ComponentBase
	{
		[Parameter]
		public int UserID { get; set; }
		private User? EditingUser { get; set; }
		private readonly Application application = Application.Instance;

		protected override async Task OnInitializedAsync()
		{
			if (UserID > 0)
			{
				EditingUser = await application.GetUserByIdAsync(dpcontext, UserID);
			}
		}

		private async Task OnAddressDeleteAsync(Address	address2delete)
		{
			address2delete.Deleted = true;
			await dpcontext.SaveAsync();
		}
		private async Task OnSaveAsync()
		{
			await dpcontext.SaveAsync();
		}

		private async Task AddAddressAsync()
		{
			if (UserID >0)
			{
				EditingUser!.AddressList.Add(new());
				await dpcontext.SaveAsync();
			}
		}

		private async Task PrimaryAddressChanged(Address changedAddress)
		{
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
					changedAddress.Primary = newValue;	// update value because binding is not yet done, we need this for saving
					await dpcontext.SaveAsync();
				}
			}
			await Task.CompletedTask;
		}
	}
}
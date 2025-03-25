using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class UserList : ComponentBase
	{
		public ICollection<Model.User> Users { get; private set; } = [];
		//[BindProperty]
		public User NewUser { get; set; } = new();

		protected override async Task OnInitializedAsync()
		{
			Users = await Application.Instance.GetAllUsersAsync(context);
			await base.OnInitializedAsync();
		}

		private async Task NewUserSubmittedAsync()
		{
			await Application.Instance.CreateUserAsync(context, NewUser);
			Users = await Application.Instance.GetAllUsersAsync(context);
		}

		private async Task OnGeoCodeAsync(User user)
		{
			Address? usersPrimaryAddress = null;
			try
			{
				usersPrimaryAddress = user.AddressList.First(a => a.Primary);
			}
			catch (InvalidOperationException)
			{
				// user has no primary address, so we take the first one
				usersPrimaryAddress = user.AddressList.First();
			}
			if (usersPrimaryAddress != null)
			{
				usersPrimaryAddress.GeoLocation = await geo.Address2LocationAsync(usersPrimaryAddress);
				await context.SaveAsync();
			}
		}
	}
}
using DA.DinnerPlanner.Common;
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
		public Model.User NewUser { get; set; } = new();

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
	}
}
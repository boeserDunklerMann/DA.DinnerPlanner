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
				EditingUser = (await application.GetAllUsersAsync(dpcontext)).First(u => u.Id == UserID);
			}
		}
	}
}
using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class EditUser : ComponentBase
	{
		[Parameter]
		public int UserID { get; set; }
		public User? EditingUser { get; set; }
		private readonly Application application = Application.Instance;
		protected override async Task OnInitializedAsync()
		{
			if (UserID > 0)
			{
				EditingUser = (await application.GetAllUsersAsync(dpcontext)).First(u => u.Id == UserID);
			}
			await base.OnInitializedAsync();
		}

		private async Task OnEditUserSubmitAsync()
		{
			await dpcontext.SaveAsync();
		}
	}
}
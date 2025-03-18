using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="18.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class EditDinnerReview : ComponentBase
	{
		[Parameter]
		public int DinnerID { get; set; }
		private Dinner? EditingDinner { get; set; }
		/// <summary>
		/// The review from the current user (we need authorization techique!)
		/// </summary>
		private DinnerReview? UsersReview { get; set; }
		protected override async Task OnInitializedAsync()
		{
			EditingDinner = await Application.Instance.GetDinnerByIdAsync(dpcontext, DinnerID);
			UsersReview = EditingDinner.Reviews.First();
		}
		private async Task SaveAsync()
		{
			await dpcontext.SaveAsync();
		}
	}
}
using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Internal;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="01.04.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class ShowGuestsPerDinner
	{
		[Parameter]
		public int DinnerID { get; set; }

		private DinnerPlannerContext? dpcontext;
		/// <summary>
		/// Identifies whether a db-action is currently in progress
		/// </summary>
		private bool Loading { get; set; } = false;
		private IQueryable<User>? GuestList { get; set; }
		private Dinner? Dinner { get; set; }
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
				Dinner = await Application.Instance.GetDinnerByIdAsync(dpcontext, DinnerID);
				GuestList = Dinner.Guests.ToList().AsQueryable();
			}
			finally
			{
				Loading = false;
			}
		}
	}
}
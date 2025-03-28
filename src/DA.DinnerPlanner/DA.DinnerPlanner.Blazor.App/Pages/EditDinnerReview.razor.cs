using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;
using System.Configuration;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="18.03.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// <Change Datum="28.03.2025" Entwickler="DA">add empty review (Jira-Nr. DPLAN-82)</Change>
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
				dpcontext.ConnectionString = configuration.GetConnectionString("da_dinnerplanner-db")!;
			}

			if (Loading)
				return;
			try
			{
				Loading = true;
				EditingDinner = await Application.Instance.GetDinnerByIdAsync(dpcontext, DinnerID);
				// if we don't have any reviews, we create and add an empty one:
				if (EditingDinner.Reviews.Count == 0)
					EditingDinner.Reviews.Add(new());
				UsersReview = EditingDinner.Reviews.FirstOrDefault();
			}
			finally
			{
				Loading = false;
			}
		}
		private async Task SaveAsync()
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
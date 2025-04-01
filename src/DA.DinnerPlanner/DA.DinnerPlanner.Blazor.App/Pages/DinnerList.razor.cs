using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Hangfire;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public partial class DinnerList : ComponentBase
	{
		private IQueryable<Dinner>? Dinners { get; set; }
		private Dinner NewDinner { get; set; } = new();
		private int NewDinnerHostId { get; set; }
		private ICollection<User> Users { get; set; } = [];
		private DinnerPlannerContext? dpcontext;
		/// <summary>
		/// Identifies whether a db-action is currently in progress
		/// </summary>
		private bool Loading { get; set; } = false;
		private string? DinnerSearch { get; set; }
		private QuickGrid<Dinner>? DinnerGrid { get; set; }
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
				Dinners = (await Application.Instance.GetAllDinnersAsync(dpcontext)).AsQueryable();
				Users = await Application.Instance.GetAllUsersAsync(dpcontext);
				await base.OnInitializedAsync();
			}
			finally 
			{
				Loading = false;
			}
		}

		private async Task NewDinnerSubmittedAsync()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				var host = Users.Single(u => u.Id == NewDinnerHostId && !u.Deleted);
				await Application.Instance.CreateDinnerAsync(dpcontext!, NewDinner, host).ConfigureAwait(true);    // DONE DA: der legt hier einen neuen User (Host) an
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(DinnerList), true);
		}

		private async Task DelDinner(Dinner dinner)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dinner.Delete();
				await dpcontext!.SaveAsync();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(DinnerList), true);
		}
		private async Task InviteGuestsAsync(Dinner dinner)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				await Application.Instance.CalculateDinerAsync(dpcontext!, dinner);
				await Application.Instance.InviteGuests4Dinner(cfg, dpcontext!, dinner);
			}
			finally
			{
				Loading = false;
			}
		}

		#region Search
		private async Task OnSearchByNameChangeAsync(ChangeEventArgs e)
		{
			DinnerSearch = e.Value?.ToString();
			if (Loading || string.IsNullOrEmpty(DinnerSearch))
				return;
			try
			{
				Loading = true;
				Dinners = await Application.Instance.SearchDinnerAsync(dpcontext!, DinnerSearch);
				await AfterSearchAsync();
			}
			finally
			{
				Loading = false;
			}
		}
		private async Task OnSearchClear()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				Dinners = await Application.Instance.SearchDinnerAsync(dpcontext!, null);
				await AfterSearchAsync();
				DinnerSearch = "";
			}
			finally
			{
				Loading = false;
			}
		}
		/// <summary>
		/// HIdes the search-inputbox
		/// </summary>
		/// <returns></returns>
		private async Task AfterSearchAsync()
		{
			await DinnerGrid!.ShowColumnOptionsAsync(null!);
		}
		#endregion

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
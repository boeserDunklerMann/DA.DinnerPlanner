using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Hangfire;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public partial class DinnerList : ComponentBase
	{
		public ICollection<Model.Dinner> Dinners { get; private set; } = [];
		public Model.Dinner NewDinner { get; set; } = new();
		public int NewDinnerHostId { get; set; }
		public ICollection<User> Users { get; set; } = [];
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
			if (Loading)
				return;
			try
			{
				Loading = true;
				Dinners = await Application.Instance.GetAllDinnersAsync(dpcontext);
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
		private void AddHangfireJob()
		{
			BackgroundJob.Enqueue<ProcessServer.GeoCoderProcess>(gc => gc.ProcessAllUsersAsync(dpcontext!.ConnectionString, geo));
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
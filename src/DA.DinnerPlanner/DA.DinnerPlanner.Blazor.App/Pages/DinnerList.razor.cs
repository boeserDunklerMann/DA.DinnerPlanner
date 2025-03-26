using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class DinnerList : ComponentBase
	{
		public ICollection<Model.Dinner> Dinners { get; private set; } = [];
		public Model.Dinner NewDinner { get; set; } = new();
		public int NewDinnerHostId { get; set; }
		public ICollection<User> Users { get; set; } = [];
		private IDinnerPlannerContext? dpcontext;
		protected override async Task OnInitializedAsync()
		{
			//	using (var dpcontext = contextFactory.CreateDbContext())
			if (dpcontext == null)
				dpcontext = await contextFactory.CreateDbContextAsync();
			dpcontext.ConnectionString = cfg.GetConnectionString("da_dinnerplanner-db")!;
			Dinners = await Application.Instance.GetAllDinnersAsync(dpcontext);
			Users = await Application.Instance.GetAllUsersAsync(dpcontext);
			await base.OnInitializedAsync();

		}

		private async Task NewDinnerSubmittedAsync()
		{
			if (dpcontext == null)
				dpcontext = await contextFactory.CreateDbContextAsync();

			dpcontext.ConnectionString = cfg.GetConnectionString("da_dinnerplanner-db")!;
			var host = Users.Single(u => u.Id == NewDinnerHostId && !u.Deleted);
			await Application.Instance.CreateDinnerAsync(dpcontext, NewDinner, host).ConfigureAwait(true);    // TODO: der legt hier einen neuen User (Host) an
			navMgr.NavigateTo(nameof(DinnerList), true);
		}

		private async Task DelDinner(Dinner dinner)
		{
			if (dpcontext == null)
				dpcontext = await contextFactory.CreateDbContextAsync();

			dpcontext.ConnectionString = cfg["ConnectionStrings:da_dinnerplanner-db"]!;
			dinner.Delete();
			await dpcontext.SaveAsync();
			navMgr.NavigateTo(nameof(DinnerList), true);
		}
		private async Task InviteGuestsAsync(Dinner dinner)
		{
			if (dpcontext == null)
				dpcontext = await contextFactory.CreateDbContextAsync();

			dpcontext.ConnectionString = cfg["ConnectionStrings:da_dinnerplanner-db"]!;
			await Application.Instance.CalculateDinerAsync(dpcontext, dinner);
			await Application.Instance.InviteGuests4Dinner(cfg, dpcontext, dinner);
		}
		private void AddHangfireJob()
		{
			if (dpcontext == null)
				dpcontext = contextFactory.CreateDbContext();
			dpcontext.ConnectionString = cfg["ConnectionStrings:da_dinnerplanner-db"]!;
			BackgroundJob.Enqueue<ProcessServer.GeoCoderProcess>(gc => gc.ProcessAllUsersAsync(dpcontext.ConnectionString, geo));
		}
	}
}
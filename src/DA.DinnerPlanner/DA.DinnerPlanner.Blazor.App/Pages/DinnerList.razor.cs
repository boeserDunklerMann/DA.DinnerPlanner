using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Hangfire;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class DinnerList : ComponentBase
	{
		public ICollection<Model.Dinner> Dinners { get; private set; } = [];
		//[BindProperty]
		public Model.Dinner NewDinner { get; set; } = new();

		protected override async Task OnInitializedAsync()
		{
			Dinners = await Application.Instance.GetAllDinnersAsync(dpcontext);
			await base.OnInitializedAsync();
		}

		private async Task NewDinnerSubmittedAsync()
		{
			await Application.Instance.CreateDinnerAsync(dpcontext, NewDinner);
			Dinners = await Application.Instance.GetAllDinnersAsync(dpcontext);
		}

		private async Task DelDinner(Dinner dinner)
		{
			dinner.Delete();
			await dpcontext.SaveAsync();
		}

		private async Task InviteGuestsAsync(Dinner dinner)
		{
			await Application.Instance.CalculateDinerAsync(dpcontext, dinner);
			await Application.Instance.InviteGuests4Dinner(cfg, dpcontext, dinner);
		}
		private void AddHangfireJob()
		{
			BackgroundJob.Enqueue<ProcessServer.GeoCoderProcess>(gc => gc.ProcessAllUsersAsync(null, dpcontext.ConnectionString, geo));
		}
	}
}
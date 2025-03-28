using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Notifications;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public partial class NotificationList : ComponentBase
	{
		[CascadingParameter]
		public int SelectedUserID { get; set; }
		public ICollection<Notification> Notifications { get; set; } = [];
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
				User ich = dpcontext.Users.First(u => u.Id == SelectedUserID);
				Notifications = [.. ich.Notifications.Where(n => !n.Deleted)];
				await Task.CompletedTask;
			}
			finally
			{
				Loading = false;
			}
		}

		private async Task OnDeleteNotificationAsync(Notification notification)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				notification.Delete();
				await dpcontext!.SaveAsync();
				navMgr.NavigateTo(nameof(NotificationList), true);
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
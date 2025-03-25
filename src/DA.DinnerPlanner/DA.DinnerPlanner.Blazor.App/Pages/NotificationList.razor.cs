using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Notifications;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class NotificationList : ComponentBase
	{
		public ICollection<Notification> Notifications { get; set; } = [];

		protected override async Task OnInitializedAsync()
		{
			User ich = dpcontext.Users.First(u => u.Id == 1);
			Notifications = [.. ich.Notifications.Where(n=>!n.Deleted)];
			await Task.CompletedTask;
		}

		private async Task OnDeleteNotificationAsync(Notification notification)
		{
			notification.Delete();
			await dpcontext.SaveAsync();
			navMgr.NavigateTo(nameof(NotificationList), true);
			await Task.CompletedTask;
		}
	}
}
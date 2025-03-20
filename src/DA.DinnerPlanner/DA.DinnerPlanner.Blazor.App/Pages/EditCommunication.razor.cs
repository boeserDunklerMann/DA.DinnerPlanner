using DA.DinnerPlanner.Blazor.App.Shared;
using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="03.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class EditCommunication : ComponentBase
	{
		[Parameter]
		public int UserID { get; set; }
		private User? EditingUser { get; set; }
		private ICollection<Communication> Communications { get; set; } = [];
		
		private readonly Application application = Application.Instance;
		private readonly Dictionary<int, CommunicationEntry> communicationEntries = [];
		protected override async Task OnInitializedAsync()
		{
			if (UserID > 0)
			{
				EditingUser = await application.GetUserByIdAsync(dpcontext, UserID);
				Communications = EditingUser.CommunicationList.Where(c => !c.Deleted).ToList();
			}
		}

		public async Task AddCommunicationAsync()
		{
			if (UserID > 0)
			{
				Communication newComm = new();
				EditingUser?.CommunicationList.Add(newComm);
				dpcontext.SaveChanges();
			}
			await Task.CompletedTask;
		}

		private async Task DeleteCommunicationAsync(int commId2Del)
		{
			if (UserID >0)
			{
				Communication? delComm = (EditingUser?.CommunicationList.First(c => c.Id == commId2Del)) ?? throw new Exception($"Communication.Id=={commId2Del} not found for User.Id=={UserID}!");
				delComm.Delete();
				dpcontext.SaveChanges();
			}
			await Task.CompletedTask;
		}

		private async Task SaveAsync()
		{
			await Task.CompletedTask;
			dpcontext.SaveChanges();
		}
	}
}
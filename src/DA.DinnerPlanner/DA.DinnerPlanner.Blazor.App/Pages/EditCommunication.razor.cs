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
				if (UserID > 0)
				{
					EditingUser = await application.GetUserByIdAsync(dpcontext, UserID);
					Communications = EditingUser.CommunicationList.Where(c => !c.Deleted).ToList();
				}
			}
			finally
			{
				Loading = false;
			}
		}

		public async Task AddCommunicationAsync()
		{
			if (Loading)
				return;
			try
			{
				if (UserID > 0)
				{
					Communication newComm = new();
					EditingUser?.CommunicationList.Add(newComm);
					dpcontext!.SaveChanges();
				}
			}
			finally
			{
				Loading = false;
			}
			await Task.CompletedTask;
		}

		private async Task DeleteCommunicationAsync(int commId2Del)
		{
			if (Loading)
				return;
			try
			{
				if (UserID > 0)
				{
					Communication? delComm = (EditingUser?.CommunicationList.First(c => c.Id == commId2Del)) ?? throw new Exception($"Communication.Id=={commId2Del} not found for User.Id=={UserID}!");
					delComm.Delete();
					dpcontext!.SaveChanges();
				}
			}
			finally
			{
				Loading = false;
			}
			await Task.CompletedTask;
		}

		private async Task SaveAsync()
		{
			if (Loading)
				return;
			try
			{
				await Task.CompletedTask;
				dpcontext!.SaveChanges();
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
using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
    /// <ChangeLog>
    /// <Create Datum="21.01.2025" Entwickler="DA" />
    /// </ChangeLog>
    public class EditCommunicationsModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context)
        : BasePageModel(config, backgroundJob, context)
    {
		[BindProperty(SupportsGet = true)]
		public int? UserID { get; set; }
		[BindProperty(SupportsGet = true)]
		public int CommID { get; set; } // will be set on delete

		public ICollection<Communication>? Communications { get; set; }

        private User? editUser;
		public async Task<IActionResult> OnGetAsync()
		{
			if (UserID == null)
                return NotFound();
            await LoadUserDataAsync();
            return Page();
		}
        public async Task<IActionResult> OnPostDeleteAsync(int userId, int commId)
        {
            await LoadUserDataAsync();
            if (editUser == null)
                return NotFound();
            editUser.CommunicationList.First(c => c.Id == CommID).Deleted = true;
            await db.SaveAsync();
            Communications!.First(c => c.Id == CommID).Deleted = true;
			return Page();
        }
        public async Task<IActionResult> OnPostAddAsync(int userId)
        {
			await LoadUserDataAsync();
			return Page();
		}

        private async Task LoadUserDataAsync()
        {
			editUser = (await application.GetAllUsersAsync(db)).First(u => u.Id == UserID);
			Communications = editUser.CommunicationList.Where(c=>!c.Deleted).ToList();
		}
	}
}
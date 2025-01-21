using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.UnitsTypes;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public SelectList? CommTypesSL { get; set; }

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
            Communication comm = editUser.CommunicationList.First(c => c.Id == CommID);
            comm.Deleted = true;
            comm.CommunicationValue = comm.CommunicationValue ?? "";
            await db.SaveAsync();
            Communications!.First(c => c.Id == CommID).Deleted = true;
			return Page();
        }
        public async Task<IActionResult> OnPostAddAsync(int userId)
        {
			await LoadUserDataAsync();
			if (editUser == null)
				return NotFound();
            editUser.CommunicationList.Add(new() { CommunicationType=await db.CommunicationTypes.FirstAsync()});
            await db.SaveAsync();
			await LoadUserDataAsync();
			return Page();
		}
        public async Task<IActionResult> OnPostSubmitAsync(int commTypeId, string commValue, int userId, int commId)
        {
			await LoadUserDataAsync();
			if (editUser == null)
				return NotFound();
            Communication comm = editUser.CommunicationList.First(c => c.Id == commId);
            comm.CommunicationValue = commValue;
            comm.CommunicationType = await db.CommunicationTypes.FirstAsync(ct => ct.Id == commTypeId);
            await db.SaveAsync();
            return Page();
		}
		private async Task LoadUserDataAsync()
        {
			editUser = (await application.GetAllUsersAsync(db)).First(u => u.Id == UserID);
			Communications = editUser.CommunicationList.Where(c=>!c.Deleted).ToList();

            var commTypesQry = db.CommunicationTypes.OrderBy(ct => ct.Name);
            CommTypesSL = new SelectList(commTypesQry, nameof(CommunicationType.Id), nameof(CommunicationType.Name));
		}
	}
}
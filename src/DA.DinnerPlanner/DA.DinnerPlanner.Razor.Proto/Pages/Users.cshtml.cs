using DA.DinnerPlanner.Model.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
	/// <ChangeLog>
	/// <Create Datum="19.12.2024" Entwickler="DA" />
	/// <Change Datum="20.12.2024" Entwickler="DA">Load users from Application class (Jira-Nr. DPLAN-15)</Change>
	/// <Change Datum="20.12.2024" Entwickler="DA">create user (Jira-Nr. DPLAN-16)</Change>
	/// <Change Datum="19.01.2025" Entwickler="DA">Hangfire support added</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">DI support (Jira-Nr. DPLAN-23)</Change>
	/// </ChangeLog>
	public class UsersModel : BasePageModel
    {
        public ICollection<Model.User> Users { get; private set; }
        [BindProperty]
        public Model.User NewUser { get; set; } = new();
        public UsersModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context) : base(config, backgroundJob, context)
        {
            // DONE DA: das hier zentral auslagern
            Users = application.GetAllUsersAsync().Result;
        }
        public async Task<IActionResult> OnPostCreateAsync()
        {
            await application.CreateUserAsync(NewUser);
            return Redirect("Index");
        }
    }
}
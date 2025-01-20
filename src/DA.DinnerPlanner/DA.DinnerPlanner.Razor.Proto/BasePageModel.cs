using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// <Change Datum="21.12.2024" Entwickler="DA">use singleton Application class (Jira-Nr. DPLAN-20)</Change>
	/// <Change Datum="19.01.2025" Entwickler="DA">Hangfire support added</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">DI support (Jira-Nr. DPLAN-23)</Change>
	/// </ChangeLog>
	public class BasePageModel : PageModel
	{
		protected Application application = Application.Instance;
		protected IDinnerPlannerContext db;
		public BasePageModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context)
		{
			backgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire"));

			db = context;
			db.ConnectionString= config["ConnectionStrings:da_dinnerplanner-db"]!;
		}
	}
}
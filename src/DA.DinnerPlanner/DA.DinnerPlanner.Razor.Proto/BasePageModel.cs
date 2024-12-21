using DA.DinnerPlanner.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// <Change Datum="21.12.2024" Entwickler="DA">use singleton Application class (Jira-Nr. DPLAN-20)</Change>
	/// </ChangeLog>
	public class BasePageModel : PageModel
	{
		//protected IDinnerPlannerContext context = new Model.DinnerPlannerContext(config["ConnectionStrings:da_dinnerplanner-db"]);
		protected Application application = Application.Instance;
		public BasePageModel(IConfiguration config)
		{
			application.SetConfig(config);
		}
	}
}
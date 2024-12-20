using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class BasePageModel(IConfiguration config) : PageModel
	{
		//protected IDinnerPlannerContext context = new Model.DinnerPlannerContext(config["ConnectionStrings:da_dinnerplanner-db"]);
		protected Application application = new(config);
	}
}
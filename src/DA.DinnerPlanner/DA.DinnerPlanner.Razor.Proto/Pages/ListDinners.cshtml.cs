using DA.DinnerPlanner.Model.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
	/// <ChangeLog>
	/// <Create Datum="19.12.2024" Entwickler="DA" />
	/// <Change Datum="19.01.2025" Entwickler="DA">Hangfire support added</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">DI support (Jira-Nr. DPLAN-23)</Change>
	/// </ChangeLog>
	public class ListDinnersModel : BasePageModel
	{
		public ICollection<Model.Dinner> Dinners { get; private set; }
		[BindProperty]
		public Model.Dinner NewDinner { get; set; } = new();

		public ListDinnersModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context) : base(config, backgroundJob, context)
		{
			Dinners = application.GetAllDinnersAsync(db).Result;
		}

		public async Task<IActionResult> OnPostCreateAsync()
		{
			//await application.CreateDinnerAsync(db, NewDinner);
			return Redirect("Index");
		}
	}
}
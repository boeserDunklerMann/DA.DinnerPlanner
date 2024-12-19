using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
	public class ListDinnersModel : BasePageModel
	{
		public IReadOnlyCollection<Model.Dinner> Dinners { get; private set; }
		[BindProperty]
		public Model.Dinner NewDinner { get; set; } = new();

		public ListDinnersModel(IConfiguration config) : base(config)
		{
			Dinners = context.Dinners.Where(d => !d.Deleted).ToList().AsReadOnly();
		}

		public async Task<IActionResult> OnPostCreateAsync()
		{
			// DONE DA: create new dinner here
			if (NewDinner != null)
			{
				context.Dinners.Add(NewDinner);
				await context.Db.SaveChangesAsync();
			}
			return Redirect("Index");
		}
	}
}
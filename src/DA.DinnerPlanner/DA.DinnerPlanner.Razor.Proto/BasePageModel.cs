using DA.DinnerPlanner.Common;
using Hangfire;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// <Change Datum="21.12.2024" Entwickler="DA">use singleton Application class (Jira-Nr. DPLAN-20)</Change>
	/// <Change Datum="19.01.2025" Entwickler="DA">Hangfire support added</Change>
	/// </ChangeLog>
	public class BasePageModel : PageModel
	{
		protected Application application = Application.Instance;
		public BasePageModel(IConfiguration config, IBackgroundJobClient backgroundJob)
		{
			application.SetConfig(config);
			backgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire"));
		}
	}
}
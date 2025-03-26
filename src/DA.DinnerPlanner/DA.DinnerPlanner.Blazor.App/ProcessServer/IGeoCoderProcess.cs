using DA.DinnerPlanner.Model.Contracts;

namespace DA.DinnerPlanner.Blazor.App.ProcessServer
{
	/// <ChangeLog>
	/// <Create Datum="25.03.2025" Entwickler="DA" />
	/// <Change Datum="26.03.2025" Entwickler="DA">Context removed from ProcessAllUsersAsync, because of DBContextFactory (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public interface IGeoCoderProcess
	{
		Task ProcessAllUsersAsync(string? ConnectionString, IGeoCoder geo);
	}
}
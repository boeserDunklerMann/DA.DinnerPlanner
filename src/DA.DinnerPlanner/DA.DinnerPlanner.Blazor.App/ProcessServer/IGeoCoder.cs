using DA.DinnerPlanner.Model.Contracts;

namespace DA.DinnerPlanner.Blazor.App.ProcessServer
{
	/// <ChangeLog>
	/// <Create Datum="25.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public interface IGeoCoder
	{
		Task ProcessAllUsersAsync(IDinnerPlannerContext? context, string? ConnectionString, Model.Contracts.IGeoCoder geo);
	}
}
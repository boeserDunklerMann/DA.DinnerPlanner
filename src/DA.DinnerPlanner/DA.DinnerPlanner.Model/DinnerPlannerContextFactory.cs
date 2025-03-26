using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="26.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class DinnerPlannerContextFactory : IDbContextFactory<DinnerPlannerContext>
	{
		public DinnerPlannerContext CreateDbContext() => new();

		public DinnerPlannerContext Create() => CreateDbContext();
	}
}

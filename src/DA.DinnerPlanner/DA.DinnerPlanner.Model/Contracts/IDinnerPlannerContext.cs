using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Model.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// <Change Datum="19.12.2024" Entwickler="DA">EatingHabits added (Jira-Nr. DPLAN-4)</Change>
	/// </ChangeLog>
	public interface IDinnerPlannerContext
	{
		DbSet<Allergy> Allergies { get; set; }
		DbSet<EatingHabit> EatingHabits { get; set; }
		DbSet<CommunicationType> CommunicationTypes { get; set; }
		DbSet<Country> Countries { get; set; }
		DbSet<Dinner> Dinners { get; set; }
		DbSet<Pet> Pets { get; set; }
		DbSet<User> Users { get; set; }
		DbContext Db { get; }
	}
}
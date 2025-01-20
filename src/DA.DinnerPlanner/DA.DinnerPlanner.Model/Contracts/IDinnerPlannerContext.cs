using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Model.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// <Change Datum="19.12.2024" Entwickler="DA">EatingHabits added (Jira-Nr. DPLAN-4)</Change>
	/// <Change Datum="19.12.2024" Entwickler="DA">Languages added (Jira-Nr. DPLAN-8)</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">DinnerImages added</Change>
	/// </ChangeLog>
	public interface IDinnerPlannerContext
	{
		DbSet<Allergy> Allergies { get; set; }
		DbSet<CommunicationType> CommunicationTypes { get; set; }
		DbSet<Country> Countries { get; set; }
		DbContext Db { get; }
		DbSet<DinnerImage> DinnerImages { get; set; }
		DbSet<Dinner> Dinners { get; set; }
		DbSet<EatingHabit> EatingHabits { get; set; }
		DbSet<Language> Languages { get; set; }
		DbSet<Pet> Pets { get; set; }
		DbSet<UserImage> UserImages { get; set; }
		DbSet<User> Users { get; set; }

		string ConnectionString { get; set; }
		Task SaveAsync();
	}
}
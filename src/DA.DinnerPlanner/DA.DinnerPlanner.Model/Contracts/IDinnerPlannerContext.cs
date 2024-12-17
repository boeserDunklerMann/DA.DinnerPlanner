using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Model.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public interface IDinnerPlannerContext
	{
		DbSet<Allergy> Allergies { get; set; }
		DbSet<CommunicationType> CommunicationTypes { get; set; }
		DbSet<Country> Countries { get; set; }
		DbSet<Dinner> Dinners { get; set; }
		DbSet<Pet> Pets { get; set; }
		DbSet<User> Users { get; set; }
	}
}
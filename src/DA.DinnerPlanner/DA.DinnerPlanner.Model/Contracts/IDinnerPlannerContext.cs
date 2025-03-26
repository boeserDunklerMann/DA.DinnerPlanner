using DA.DinnerPlanner.Model.Auth;
using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Model.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// <Change Datum="19.12.2024" Entwickler="DA">EatingHabits added (Jira-Nr. DPLAN-4)</Change>
	/// <Change Datum="19.12.2024" Entwickler="DA">Languages added (Jira-Nr. DPLAN-8)</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">DinnerImages added</Change>
	/// <Change Datum="27.01.2025" Entwickler="DA">GoogleUsers added (Jira-Nr. DPLAN-38)</Change>
	/// <Change Datum="23.02.2025" Entwickler="DA">Roles added (Jira-Nr. DPLAN-44)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">Notifications added (Jira-Nr. DPLAN-68)</Change>
	/// </ChangeLog>
	public interface IDinnerPlannerContext
	{
		DbSet<Allergy> Allergies { get; set; }
		DbSet<CommunicationType> CommunicationTypes { get; set; }
		DbSet<Country> Countries { get; set; }
		DbSet<DinnerImage> DinnerImages { get; set; }
		DbSet<Dinner> Dinners { get; set; }
		DbSet<EatingHabit> EatingHabits { get; set; }
		DbSet<Language> Languages { get; set; }
		DbSet<Pet> Pets { get; set; }
		DbSet<UserImage> UserImages { get; set; }
		DbSet<User> Users { get; set; }
		DbSet<Auth.GoogleUser> GoogleUsers { get; set; }
		DbSet<Role> Roles { get; set; }
		DbSet<Notifications.Notification> Notifications { get; set; }
		string ConnectionString { get; set; }
		Task SaveAsync();
		int SaveChanges();
	}
}
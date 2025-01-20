using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Razor.Proto.ViewModel;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Razor.Proto
{
	/// <remarks>
	/// see here for binding lists: https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/update-related-data?view=aspnetcore-8.0
	/// </remarks>
	/// <ChangeLog>
	/// <Create Datum="17.12.2024" Entwickler="DA" />
	/// <Change Datum="21.12.2024" Entwickler="DA">use singleton Application class (Jira-Nr. DPLAN-20)</Change>
	/// <Change Datum="19.01.2025" Entwickler="DA">Hangfire support added</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">DI support (Jira-Nr. DPLAN-23)</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">PopulateUsersAllergies added (Jira-Nr. DPLAN-12)</Change>
	/// </ChangeLog>
	public class BasePageModel : PageModel
	{
		protected Application application = Application.Instance;
		protected IDinnerPlannerContext db;
		public BasePageModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context)
		{
			//backgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire"));

			db = context;
			db.ConnectionString = config["ConnectionStrings:da_dinnerplanner-db"]!;
		}
		#region stuff for editing user
		/// <summary>
		/// List of (checkable) allergies for the user
		/// </summary>
		public List<AssignedAllergyData> UsersAllergies { get; set; } = [];

		/// <summary>
		/// List of (checkable) languages for the user
		/// </summary>
		public List<AssignedLanguageData> UsersLanguages { get; set; } = [];
		public List<AssignedPetsData> UsersPets { get; set; } = [];

		protected async Task PopulateUsersAllergiesAsync(User editUser)
		{
			UsersAllergies = [];
			await db.Allergies.ForEachAsync(allergy =>
			{
				UsersAllergies.Add(new()
				{
					AllergyID = allergy.Id,
					Name = allergy.Name,
					Assigned = editUser.Allergies.Any(usersallergy => usersallergy.Id == allergy.Id)
				});
			});
		}

		protected async Task PopulateUsersLanguagesAsync(User editUser)
		{
			UsersLanguages = [];
			await db.Languages.ForEachAsync(lang =>
			{
				UsersLanguages.Add(new()
				{
					LanguageId = lang.Id,
					Name = lang.Name,
					Assigned = editUser.Languages.Any(userslang => userslang.Id == lang.Id)
				});
			});
		}

		protected async Task PopulateUsersPetsAsync(User editUser)
		{
			UsersPets = [];
			await db.Pets.ForEachAsync(pet =>
			{
				UsersPets.Add(new()
				{
					PetId = pet.Id,
					Name = pet.Name,
					Assigned = editUser.Pets.Any(userspet => userspet.Id == pet.Id)
				});
			});
		}

		protected void UpdateAllergiesFromBinding(User editUser, string[] selectedAllergies)
		{
			// TODO DA: finde raus, hat sich hier überhaupt was geändert, sonst kommt ne PK-Exception

			editUser.Allergies.Clear();
			foreach (var allergyId in selectedAllergies.ToList().AsReadOnly())
			{
				int aID = int.Parse(allergyId);
				editUser.Allergies.Add(db.Allergies.First(a => a.Id == aID));
			}
		}

		protected void UpdateLanguagesFromBinding(User editUser, string[] selectedLanguages)
		{
			// TODO DA: finde raus, hat sich hier überhaupt was geändert, sonst kommt ne PK-Exception
			editUser.Languages.Clear();
			foreach (var langID in selectedLanguages.ToList().AsReadOnly())
			{
				int lId = int.Parse(langID);
				editUser.Languages.Add(db.Languages.First(l => l.Id == lId));
			}
		}

		protected void UpdatePetsFromBinding(User editUser, string[] selectedPets)
		{
			// TODO DA: finde raus, hat sich hier überhaupt was geändert, sonst kommt ne PK-Exception
			editUser.Pets.Clear();
			foreach (var petId in selectedPets.ToList().AsReadOnly())
			{
				int pId = int.Parse(petId);
				editUser.Pets.Add(db.Pets.First(p => p.Id == pId));
			}
		}
		#endregion
	}
}
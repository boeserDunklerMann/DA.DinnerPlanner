using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
	public class EditUserModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context) : BasePageModel(config, backgroundJob, context)
    {
        [BindProperty(SupportsGet = true)]
        public int UserID { get; set; }
        [BindProperty]
        public User? EditUser { get; set; }

        public async Task OnGetAsync()
        {
            var allUsers = await application.GetAllUsersAsync(db);
            EditUser = allUsers.First(u => u.Id == UserID);
            await PopulateUsersAllergiesAsync(EditUser);
            await PopulateUsersLanguagesAsync(EditUser);
            await PopulateUsersPetsAsync(EditUser);
        }

        public async Task<IActionResult> OnPostEditSubmitAsync(int? id,
            string firstName, string lastName, DateTime birthDate, bool asCook,
			string[] selectedAllergies,
            string[] selectedLanguages,
            string[] selectedPets)
        {
            if (id == null)
                return NotFound();
            EditUser = (await application.GetAllUsersAsync(db)).First(u => u.Id == id);
            EditUser.FirstName = firstName;
            EditUser.LastName = lastName;
            EditUser.BirthDate = birthDate;
            EditUser.AvailableAsCook = asCook;

			UpdateAllergiesFromBinding(EditUser, selectedAllergies);
            UpdateLanguagesFromBinding(EditUser, selectedLanguages);
            UpdatePetsFromBinding(EditUser, selectedPets);

            await db.SaveAsync();
            await PopulateUsersAllergiesAsync(EditUser);
			await PopulateUsersLanguagesAsync(EditUser);
			await PopulateUsersPetsAsync(EditUser);

			return Redirect("Users");
        }
    }
}

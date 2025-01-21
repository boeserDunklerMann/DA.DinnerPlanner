using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.UnitsTypes;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
	public class EditUserModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context) : BasePageModel(config, backgroundJob, context)
    {
        [BindProperty(SupportsGet = true)]
        public int UserID { get; set; }
        public User? EditUser { get; set; }

        public SelectList? EatingHabitSL { get; set; }

        private void PopulateEatinghabitDropDownList(object? selectedItem=null)
        {
            var eatinghabitsQuery = from eh in db.EatingHabits
                                    orderby eh.Name
                                    select eh;
            EatingHabitSL = new SelectList(eatinghabitsQuery.AsNoTracking(), nameof(EatingHabit.Id), nameof(EatingHabit.Name), selectedItem);
        }
        public async Task OnGetAsync()
        {
            var allUsers = await application.GetAllUsersAsync(db);
            EditUser = allUsers.First(u => u.Id == UserID);
            await PopulateUsersAllergiesAsync(EditUser);
            await PopulateUsersLanguagesAsync(EditUser);
            await PopulateUsersPetsAsync(EditUser);
            PopulateEatinghabitDropDownList(EditUser.EatingHabit?.Id);
        }

        public async Task<IActionResult> OnPostEditSubmitAsync(int? id,
            string firstName, string lastName, DateTime birthDate, bool asCook,
            string userComment, int eatingHabitId,
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
            EditUser.UsersComment = userComment??"";
            EditUser.EatingHabit = await db.EatingHabits.FirstAsync(eh => eh.Id == eatingHabitId);

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

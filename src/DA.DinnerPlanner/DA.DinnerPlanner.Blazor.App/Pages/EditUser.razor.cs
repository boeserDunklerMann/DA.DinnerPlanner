using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// <Change Datum="28.02.2025" Entwickler="DA">Carrier classes added for Checkboxes and select list</Change>
	/// </ChangeLog>
	public partial class EditUser : ComponentBase
	{
		[Parameter]
		public int UserID { get; set; }
		public User? EditingUser { get; set; }
		private readonly Application application = Application.Instance;
		protected override async Task OnInitializedAsync()
		{
			if (UserID > 0)
			{
				EditingUser = (await application.GetAllUsersAsync(dpcontext)).First(u => u.Id == UserID);
				await LoadCheckboxesAsync();
			}
			await base.OnInitializedAsync();
		}

		private ICollection<AllergyCarrier> Allergies { get; set; } = [];
		private ICollection<LanguageCarrier> Languages { get; set; } = [];
		private ICollection<PetCarrier> Pets { get; set; } = [];
		private async Task LoadCheckboxesAsync()
		{
			await dpcontext.Allergies.ForEachAsync(a => Allergies.Add(new() { Allergy = a, Selected = EditingUser!.Allergies.Contains(a) }));
			await dpcontext.Languages.ForEachAsync(l => Languages.Add(new() { Language = l, Selected = EditingUser!.Languages.Contains(l) }));
			await dpcontext.Pets.ForEachAsync(p => Pets.Add(new() { Pet = p, Selected = EditingUser!.Pets.Contains(p) }));
		}

		#region checkbox-toggles
		private async Task ToggleAllergyAsync(ChangeEventArgs args, AllergyCarrier allergy)
		{
			if ((bool)args.Value!)
				EditingUser!.Allergies.Add(allergy.Allergy!);
			else
				EditingUser!.Allergies.Remove(allergy.Allergy!);

			await Task.CompletedTask;
		}
		private async Task ToggleLanguageAsync(ChangeEventArgs args, LanguageCarrier language)
		{
			if ((bool)args.Value!)
				EditingUser!.Languages.Add(language.Language!);
			else
				EditingUser!.Languages.Remove(language.Language!);

			await Task.CompletedTask;
		}
		private async Task TogglePetAsync(ChangeEventArgs args, PetCarrier pet)
		{
			if ((bool)args.Value!)
				EditingUser!.Pets.Add(pet.Pet!);
			else
				EditingUser!.Pets.Remove(pet.Pet!);

			await Task.CompletedTask;
		}
		#endregion

		public bool HideSavedLabel { get; set; } = true;

		private void OnEditUserSubmit()
		{
			dpcontext.SaveChanges();
			HideSavedLabel = false;
		}

		#region Carrier classes
		internal class AllergyCarrier
		{
			public bool Selected { get; set; }
			public Model.UnitsTypes.Allergy? Allergy { get; set; }
		}
		private class PetCarrier
		{
			public bool Selected { get; set; }
			public Model.Pet? Pet { get; set; }
		}
		private class LanguageCarrier
		{
			public bool Selected { get; set; }
			public Model.UnitsTypes.Language? Language { get; set; }
		}

		#endregion
	}
}
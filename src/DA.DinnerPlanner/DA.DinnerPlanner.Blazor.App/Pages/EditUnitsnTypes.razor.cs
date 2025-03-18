using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="18.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class EditUnitsnTypes : ComponentBase
	{
		private string NewAllergy { get; set; } = "";
		private string NewCommType { get; set; } = "";
		private string NewCountry { get; set; } = "";
		private string NewCountryCode { get; set; } = "";
		private string NewEatingHabit { get; set; } = "";
		private string NewLanguage { get; set; } = "";
		private string NewPet { get; set; } = "";

		#region Add/Del actions
		private void AddAllergy()
		{
			dpcontext.Allergies.Add(new() { Name = NewAllergy });
			dpcontext.SaveChanges();
			NewAllergy = "";
		}
		private void DelAllergy(Allergy allergy)
		{
			allergy.Deleted = true;
			dpcontext.SaveChanges();
		}
		private void DelCommType(CommunicationType communicationType)
		{
			communicationType.Deleted = true;
			dpcontext.SaveChanges();
		}
		private void AddCommType()
		{
			dpcontext.CommunicationTypes.Add(new() { Name = NewCommType });
			dpcontext.SaveChanges();
			NewCommType = "";
		}
		private void AddCountry()
		{
			dpcontext.Countries.Add(new() { CountryName = NewCountry, CountryCode = NewCountryCode });
			dpcontext.SaveChanges();
			NewCountry = NewCountryCode = "";
		}
		private void DelCountry(Country country)
		{
			country.Deleted = true;
			dpcontext.SaveChanges();
		}
		private void AddEatingHabit()
		{
			dpcontext.EatingHabits.Add(new() { Name = NewEatingHabit });
			dpcontext.SaveChanges();
			NewEatingHabit = "";
		}
		private void DelEatingHabit(EatingHabit habit)
		{
			habit.Deleted = true;
			dpcontext.SaveChanges();
		}
		private void AddLanguage()
		{
			dpcontext.Languages.Add(new() { Name = NewLanguage });
			dpcontext.SaveChanges();
			NewLanguage = "";
		}
		private void DelLanguage(Language lang)
		{
			lang.Deleted = true;
			dpcontext.SaveChanges();
		}
		private void AddPet()
		{
			dpcontext.Pets.Add(new() { Name = NewPet });
			dpcontext.SaveChanges();
			NewPet = "";
		}
		private void DelPet(Pet pet)
		{
			pet.Deleted= true;
			dpcontext.SaveChanges();
		}

		private void Save()
		{
			dpcontext.SaveChanges();
		}
		#endregion
	}
}
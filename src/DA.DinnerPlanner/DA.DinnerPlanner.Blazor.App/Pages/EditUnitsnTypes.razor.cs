using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="18.03.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
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

		#region MyRegion Collections for UI
		private ICollection<Allergy> Allergies { get; set; } = [];
		private ICollection<CommunicationType> CommunicationTypes { get; set; } = [];
		private ICollection<Country> Countries { get; set; } = [];
		private ICollection<EatingHabit> EatingHabits { get; set; } = [];
		private ICollection<Language> Languages { get; set; } = [];
		private ICollection<Pet> Pets { get; set; } = [];
		#endregion

		private DinnerPlannerContext? dpcontext;
		/// <summary>
		/// Identifies whether a db-action is currently in progress
		/// </summary>
		private bool Loading { get; set; } = false;

		protected override async Task OnInitializedAsync()
		{
			if (dpcontext == null)
			{
				dpcontext = await contextFactory.CreateDbContextAsync();
				dpcontext.ConnectionString = configuration.GetConnectionString("da_dinnerplanner-db")!;
			}
			if (Loading)
				return;
			try
			{
				Loading = true;
				Allergies = [.. dpcontext.Allergies.Where(a => !a.Deleted)];
				CommunicationTypes = [.. dpcontext.CommunicationTypes.Where(a => !a.Deleted)];
				Countries = [.. dpcontext.Countries.Where(a => !a.Deleted)];
				EatingHabits = [.. dpcontext.EatingHabits.Where(x => !x.Deleted)];
				Languages = [.. dpcontext.Languages.Where(x => !x.Deleted)];
				Pets = [.. dpcontext.Pets.Where(x => !x.Deleted)];
			}
			finally
			{
				Loading = false;
			}
		}

		#region Add/Del actions
		private void AddAllergy()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dpcontext!.Allergies.Add(new() { Name = NewAllergy });
				dpcontext.SaveChanges();
				NewAllergy = "";
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void DelAllergy(Allergy allergy)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				allergy.Delete();
				dpcontext!.SaveChanges();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void DelCommType(CommunicationType communicationType)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				communicationType.Delete();
				dpcontext!.SaveChanges();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void AddCommType()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dpcontext!.CommunicationTypes.Add(new() { Name = NewCommType });
				dpcontext.SaveChanges();
				NewCommType = "";
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void AddCountry()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dpcontext!.Countries.Add(new() { CountryName = NewCountry, CountryCode = NewCountryCode });
				dpcontext.SaveChanges();
				NewCountry = NewCountryCode = "";
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void DelCountry(Country country)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				country.Delete();
				dpcontext!.SaveChanges();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void AddEatingHabit()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dpcontext!.EatingHabits.Add(new() { Name = NewEatingHabit });
				dpcontext.SaveChanges();
				NewEatingHabit = "";
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void DelEatingHabit(EatingHabit habit)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				habit.Delete();
				dpcontext!.SaveChanges();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void AddLanguage()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dpcontext!.Languages.Add(new() { Name = NewLanguage });
				dpcontext.SaveChanges();
				NewLanguage = "";
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void DelLanguage(Language lang)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				lang.Delete();
				dpcontext!.SaveChanges();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void AddPet()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dpcontext!.Pets.Add(new() { Name = NewPet });
				dpcontext.SaveChanges();
				NewPet = "";
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		private void DelPet(Pet pet)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				pet.Delete();
				dpcontext!.SaveChanges();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}

		private void Save()
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				dpcontext!.SaveChanges();
			}
			finally
			{
				Loading = false;
			}
			navMgr.NavigateTo(nameof(EditUnitsnTypes), true);
		}
		#endregion

		#region Disposing
		// see: https://learn.microsoft.com/de-de/dotnet/fundamentals/code-analysis/quality-rules/ca1816#example-that-satisfies-ca1816

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				dpcontext?.Dispose();
				dpcontext = null;
			}
		}
		#endregion
	}
}
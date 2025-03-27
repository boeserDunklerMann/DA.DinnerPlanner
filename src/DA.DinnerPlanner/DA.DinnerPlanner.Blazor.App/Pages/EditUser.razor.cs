using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// <Change Datum="28.02.2025" Entwickler="DA">Carrier classes added for Checkboxes and select list</Change>
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public partial class EditUser : ComponentBase
	{
		[Parameter]
		public int UserID { get; set; }
		public User? EditingUser { get; set; }
		private readonly Application application = Application.Instance;
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
				dpcontext.ConnectionString = cfg.GetConnectionString("da_dinnerplanner-db")!;
			}

			if (Loading)
				return;
			try
			{
				if (UserID > 0)
				{
					EditingUser = await application.GetUserByIdAsync(dpcontext, UserID);
					await LoadCheckboxesAsync();
				}
			}
			finally
			{
				Loading = false;
			}
			await base.OnInitializedAsync();
		}

		private ICollection<AllergyCarrier> Allergies { get; set; } = [];
		private ICollection<LanguageCarrier> Languages { get; set; } = [];
		private ICollection<PetCarrier> Pets { get; set; } = [];
		private async Task LoadCheckboxesAsync()
		{
			if (Loading)
				return;
			try
			{
				await dpcontext!.Allergies.ForEachAsync(a => Allergies.Add(new() { Allergy = a, Selected = EditingUser!.Allergies.Contains(a) }));
				await dpcontext!.Languages.ForEachAsync(l => Languages.Add(new() { Language = l, Selected = EditingUser!.Languages.Contains(l) }));
				await dpcontext!.Pets.ForEachAsync(p => Pets.Add(new() { Pet = p, Selected = EditingUser!.Pets.Contains(p) }));
			}
			finally
			{
				Loading = false;
			}
		}

		#region checkbox-toggles
		private async Task ToggleAllergyAsync(ChangeEventArgs args, AllergyCarrier allergy)
		{
			if (Loading)
				return;
			try
			{
				if ((bool)args.Value!)
					EditingUser!.Allergies.Add(allergy.Allergy!);
				else
					EditingUser!.Allergies.Remove(allergy.Allergy!);
			}
			finally
			{
				Loading = false;
			}
			await Task.CompletedTask;
		}
		private async Task ToggleLanguageAsync(ChangeEventArgs args, LanguageCarrier language)
		{
			if (Loading)
				return;
			try
			{
				if ((bool)args.Value!)
					EditingUser!.Languages.Add(language.Language!);
				else
					EditingUser!.Languages.Remove(language.Language!);
			}
			finally
			{
				Loading = false;
			}
			await Task.CompletedTask;
		}
		private async Task TogglePetAsync(ChangeEventArgs args, PetCarrier pet)
		{
			if (Loading)
				return;
			try
			{
				if ((bool)args.Value!)
					EditingUser!.Pets.Add(pet.Pet!);
				else
					EditingUser!.Pets.Remove(pet.Pet!);
			}
			finally
			{
				Loading = false;
			}
			await Task.CompletedTask;
		}
		#endregion

		public bool HideSavedLabel { get; set; } = true;

		private void OnEditUserSubmit()
		{
			if (Loading)
				return;
			try
			{
				dpcontext!.SaveChanges();
				HideSavedLabel = false;
			}
			finally
			{
				Loading = false;
			}
		}

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
using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DA.DinnerPlanner.Blazor.App.Shared
{
	/// <ChangeLog>
	/// <Create Datum="17.03.2025" Entwickler="DA" />
	/// <Change Datum="18.03.2025" Entwickler="DA">event OnPrimaryChangedEvent added</Change>
	/// </ChangeLog>
	public partial class AddressEntry : ComponentBase
	{
		#region Parameters
		[Parameter]
		public int UserID { get; set; }
		[Parameter]
		public Address? Address { get; set; }
		[Parameter]
		public ICollection<Country>? Countries { get; set; }
		[Parameter]
		public EventCallback<Address> OnPrimaryChangedEvent { get; set; }
		#endregion
		private int CountryId
		{
			get => Address!.Country.Id;
			set => Address!.Country = Countries!.Single(c => c.Id == value && !c.Deleted);
		}
		private async Task OnPrimaryChangeAsync()
		{
			await OnPrimaryChangedEvent.InvokeAsync(Address!);
		}
	}
}
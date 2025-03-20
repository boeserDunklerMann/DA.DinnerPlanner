using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.UnitsTypes;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
	/// <ChangeLog>
	/// <Create Datum="22.01.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class EditAddressesModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context) :
		BasePageModel(config, backgroundJob, context)
	{
		[BindProperty(SupportsGet = true)]
		public int? UserID { get; set; }
		private User? editUser;
		public ICollection<Address>? Addresses { get; set; }
		public SelectList? CountriesSL { get; set; }
		public async Task<IActionResult> OnGetAsync()
		{
			if (UserID == null)
				return NotFound();
			await LoadUserDataAsync();
			return Page();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="primary">Wenn checked, dann ist das ein Array mit größe 1, in welchem die Addr.Id des Primäreintrags steht</param>
		/// <returns></returns>
		public async Task<IActionResult> OnPostSubmitAsync(int userId, int addrId, string Street, string HouseNumber, string HouseNumberExt,
			string zipCode, string city, int countryId, int[] primary)
		{
			await LoadUserDataAsync();
			if (editUser == null)
				return NotFound();
			Address addr = editUser.AddressList.First(address => address.Id == addrId);
			addr.Street = Street ?? "";
			addr.HouseNumber = HouseNumber ?? "";
			addr.HouseNumberExtension = HouseNumberExt ?? "";
			addr.ZipCode = zipCode ?? "";
			addr.City = city ?? "";
			addr.Country = await db.Countries.FirstAsync(c => c.Id == countryId);
			if (primary.Length > 0)
				addr.Primary = primary[0] == addr.Id;
			else
				addr.Primary = false;
			await db.SaveAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostAddAsync(int userId)
		{
			await LoadUserDataAsync();
			if (editUser == null)
				return NotFound();
			editUser.AddressList.Add(new() { Country = await db.Countries.FirstAsync(), Primary = false });
			await db.SaveAsync();
			await LoadUserDataAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostDeleteAsync(int userId, int addrId)
		{
			await LoadUserDataAsync();
			if (editUser == null)
				return NotFound();
			Address address = editUser.AddressList.First(a => a.Id == addrId);
			address.Delete();
			await db.SaveAsync();
			Addresses!.First(a => a.Id == addrId).Delete();
			return Page();
		}

		private async Task LoadUserDataAsync()
		{
			editUser = (await application.GetAllUsersAsync(db)).First(u => u.Id == UserID);
			Addresses = editUser.AddressList.Where(a => !a.Deleted).ToList();

			CountriesSL = new SelectList(db.Countries.OrderBy(c => c.CountryName).Where(c => !c.Deleted),
				nameof(Country.Id), nameof(Country.CountryName));
		}
	}
}
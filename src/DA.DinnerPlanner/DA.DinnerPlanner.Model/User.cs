using DA.DinnerPlanner.Model.UnitsTypes;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class User : BaseModel
	{
		public string GoogleId { get; set; } = "";
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public string DisplayName { get; set; } = "";
		public DateOnly BirthDate { get; set; }
		/// <summary>
		/// a user can have more then one address
		/// </summary>
		public ICollection<Address> AddressList { get; set; } = new List<Address>();
		public ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
		public override bool Equals(object? obj)
		{
			if (obj ==null || !(obj is User)) return false;
			return Id == ((User)obj).Id;
		}
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
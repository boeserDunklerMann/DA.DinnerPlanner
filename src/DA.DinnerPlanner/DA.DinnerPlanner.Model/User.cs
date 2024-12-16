using DA.DinnerPlanner.Model.UnitsTypes;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class User : BaseModel
	{
		public string? GoogleId { get; set; }
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public string DisplayName { get; set; } = "";
		public DateTime BirthDate { get; set; }
		/// <summary>
		/// Something like:
		/// Ich habe einen riesen Palast als Gastgeber aber ich kann nicht kochen!
		/// </summary>
		public string UsersComment { get; set; } = "";
		/// <summary>
		/// a user can have more then one address
		/// </summary>
		public ICollection<Address> AddressList { get; set; } = [];
		public ICollection<Allergy> Allergies { get; set; } = [];
		public ICollection<Communication> CommunicationList { get; set; } = [];
		public ICollection<Dinner> DinnerAsHost { get; set; } = [];
		public ICollection<Dinner> DinnerAsCook { get; set; } = [];
		public ICollection<Dinner> DinnerAsGuest {  get; set; } = [];
		public ICollection<DinnerReview> Reviews { get; set; } = [];
		public ICollection<Pet> Pets { get; set; } = [];

		public override bool Equals(object? obj)
		{
			if (obj ==null || !(obj is User)) return false;
			return Id == ((User)obj).Id;
		}
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
		public string GetDefaultDisplayName()
		{
			return $"{LastName}, {FirstName}";
		}
	}
}
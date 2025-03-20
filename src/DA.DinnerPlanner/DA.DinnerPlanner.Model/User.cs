using DA.DinnerPlanner.Model.UnitsTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="19.12.2024" Entwickler="DA">EatingHabits added (Jira-Nr. DPLAN-4)</Change>
	/// <Change Datum="19.12.2024" Entwickler="DA">Smoker added (Jira-Nr. DPLAN-5)</Change>
	/// <Change Datum="19.12.2024" Entwickler="DA">Languages added (Jira-Nr. DPLAN-8)</Change>
	/// <Change Datum="19.12.2024" Entwickler="DA">UserImages added (Jira-Nr. DPLAN-9)</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">prop AvailableAsCook added</Change>
	/// <Change Datum="23.02.2025" Entwickler="DA">prop Role added (Jira-Nr. DPLAN-44)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">added LazyLoading support (virtal) (Jira-Nr. DPLAN-63)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
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
		public virtual ICollection<Address> AddressList { get; set; } = [];
		public virtual ICollection<Allergy> Allergies { get; set; } = [];
		public virtual ICollection<Communication> CommunicationList { get; set; } = [];
		public virtual ICollection<Dinner> DinnerAsHost { get; set; } = [];
		public virtual ICollection<Dinner> DinnerAsCook { get; set; } = [];
		public virtual ICollection<Dinner> DinnerAsGuest { get; set; } = [];
		public virtual ICollection<DinnerReview> Reviews { get; set; } = [];
		public virtual ICollection<Pet> Pets { get; set; } = [];
		public virtual EatingHabit? EatingHabit { get; set; }
		public bool? Smoker { get; set; }
		public virtual ICollection<UserImage> UserImages { get; set; } = [];
		[NotMapped]
		public int Age => (int)(Math.Floor((DateTime.UtcNow - BirthDate).TotalDays / 365));
		/// <summary>
		/// Is this user also assignable as cook
		/// </summary>
		public bool AvailableAsCook { get; set; }
		public virtual Auth.Role? Role { get; set; }
		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not User) return false;
			return Id == ((User)obj).Id;
		}
		public virtual ICollection<Language> Languages { get; set; } = [];

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
		public string GetDefaultDisplayName()
		{
			return $"{LastName}, {FirstName}";
		}

		public override void Delete()
		{
			throw new NotImplementedException();
		}
	}

	/// <ChangeLog>
	/// <Create Datum="19.12.2024" Entwickler="DA" />
	/// <Change Datum="13.02.2025" Entwickler="DA">GetHashCode added</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	public class UserImage : BaseModel
	{
		public byte[] Image { get; set; } = [];
		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not UserImage) return false;
			return Id == ((BaseModel)obj).Id;
		}
		public virtual User User { get; set; } = new();

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override void Delete()
		{
			throw new NotImplementedException();
		}
	}
}
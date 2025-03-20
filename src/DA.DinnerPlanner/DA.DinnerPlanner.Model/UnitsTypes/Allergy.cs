using System.Text.Json.Serialization;

namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	/// <summary>
	/// a possible allergy a user could have
	/// </summary>
	public class Allergy : BaseModel
	{
		public string Name { get; set; } = "";
		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not Allergy) return false;
			return Id == ((Allergy)obj).Id;
		}

		public override void Delete()
		{
			if (Users.Count > 0)
				throw new Exceptions.DeleteReferenceException($"AllergyId {Id} is in use by {nameof(User)}");
			Deleted = true;
		}

		public virtual ICollection<User> Users { get; set; } = [];  // see here https://learn.microsoft.com/de-de/ef/core/modeling/relationships/many-to-many
	}
}
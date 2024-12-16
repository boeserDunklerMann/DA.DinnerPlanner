using System.Text.Json.Serialization;

namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// a possible allergy a user could have
	/// </summary>
	public class Allergy : BaseModel
	{
		public string Name { get; set; } = "";
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Allergy)) return false;
			return Id == ((Allergy)obj).Id;
		}
	}
}
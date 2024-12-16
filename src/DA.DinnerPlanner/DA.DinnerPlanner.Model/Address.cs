using DA.DinnerPlanner.Model.UnitsTypes;
using System.Text.Json.Serialization;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class Address:BaseModel
	{
		public string Street { get; set; } = "";
		public string HouseNumber { get; set; } = "";
		public string HouseNumberExtension { get; set; } = "";
		public string City { get; set; } = "";
		public string ZipCode { get; set; } = "";
		public Country Country { get; set; } = new Country();
		[JsonIgnore]
		public virtual User User { get; set; }
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Address)) return false;
			return Id.Equals(((Address)obj).Id);
		}
	}
}

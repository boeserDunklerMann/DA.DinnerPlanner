using DA.DinnerPlanner.Model.GeoCode;
using DA.DinnerPlanner.Model.UnitsTypes;
using System.Text.Json.Serialization;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="22.01.2025" Entwickler="DA">property Primary added (Jira-Nr. DPLAN-25)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">added LazyLoading support (virtal) (Jira-Nr. DPLAN-63)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// <Change Datum="24.03.2025" Entwickler="DA">GetHashCode overridden</Change>
	/// <Change Datum="24.03.2025" Entwickler="DA">GeoLocation added (Jira-Nr. DPLAN-73)</Change>
		/// </ChangeLog>
	public class Address : BaseModel
	{
		public string Street { get; set; } = "";
		public string HouseNumber { get; set; } = "";
		public string HouseNumberExtension { get; set; } = "";
		public string City { get; set; } = "";
		public string ZipCode { get; set; } = "";
		public virtual Country Country { get; set; } = new Country();
		public virtual GeoLocation? GeoLocation { get; set; }
		/// <summary>
		/// Hauptwohnsitz/-adresse
		/// </summary>
		public bool Primary { get; set; }
		[JsonIgnore]
		public virtual User User { get; set; } = new();

		public override void Delete()
		{
			Deleted = true;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not Address) return false;
			return Id.Equals(((Address)obj).Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
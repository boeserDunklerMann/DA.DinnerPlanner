using DA.DinnerPlanner.Model.UnitsTypes;
using System.Text.Json.Serialization;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">added LazyLoading support (virtal) (Jira-Nr. DPLAN-63)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	public class Communication : BaseModel
	{
		public virtual CommunicationType CommunicationType { get; set; }=new CommunicationType();
		/// <summary>
		/// the communication value itself, like mailaddress, mobilenumber, ...
		/// </summary>
		public string CommunicationValue { get; set; } = "";
		[JsonIgnore]
		public virtual User? User { get; set; }

		public override void Delete()
		{
			Deleted = true;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not Communication) return false;
			return Id == ((Communication)obj).Id;
		}
	}
}

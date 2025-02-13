using DA.DinnerPlanner.Model.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="13.02.2025" Entwickler="DA">implements ICurrentTimestamps (Jira-Nr. DPLAN-41)</Change>
	/// </ChangeLog>
	/// <remarks>
	/// all Dates are stored in Utc since we are an international app.
	/// </remarks>
	public abstract class BaseModel : ICurrentTimestamps
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public bool Deleted { get; set; }
		public DateTime? ChangeDate { get; set; }
		public DateTime CreationDate { get; set; }

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}

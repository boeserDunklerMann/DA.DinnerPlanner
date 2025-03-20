using DA.DinnerPlanner.Model.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="13.02.2025" Entwickler="DA">implements ICurrentTimestamps (Jira-Nr. DPLAN-41)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">abstract method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	/// <remarks>
	/// all Dates are stored in Utc since we are an international app.
	/// </remarks>
	public abstract class BaseModel : ICurrentTimestamps
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public bool Deleted { get; protected set; }
		public DateTime? ChangeDate { get; set; }
		public DateTime CreationDate { get; set; }

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
		/// <summary>
		/// Deletes an object and checks all references firsst
		/// </summary>
		public abstract void Delete();
	}
}
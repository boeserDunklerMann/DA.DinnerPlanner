using System.ComponentModel.DataAnnotations.Schema;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	/// <remarks>
	/// all Dates are stored in Utc since we are an international app.
	/// </remarks>
	public abstract class BaseModel
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		/// <summary>
		/// Änderungsdatum
		/// </summary>
		public DateTime? ChangeDate { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Erstelldatum
		/// </summary>
		public DateTime? CreationDate { get; set; }=DateTime.UtcNow;
		public bool Deleted { get; set; }
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public DateTime? ChangeDate { get; set; }

		/// <summary>
		/// Erstelldatum
		/// </summary>
		public DateTime? CreationDate { get; set; }
		public bool Deleted { get; set; }
		public static T Create<T>() where T : BaseModel, new()
			=> new T { ChangeDate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Deleted = false };
	}
}

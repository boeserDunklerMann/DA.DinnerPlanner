using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="19.12.2024" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	public class EatingHabit : BaseModel
	{
		public string Name { get; set; } = "";
		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not EatingHabit) return false;
			return Id == ((EatingHabit)obj).Id;
		}

		public override void Delete()
		{
			if (Users.Count > 0)
				throw new Exceptions.DeleteReferenceException($"EatingHabitId {Id} is in use by {nameof(User)}");
		}

		public virtual ICollection<User> Users { get; set; } = [];
	}
}

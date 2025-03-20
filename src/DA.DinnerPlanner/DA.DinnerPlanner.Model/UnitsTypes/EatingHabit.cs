using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="19.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class EatingHabit : BaseModel
	{
		public string Name { get; set; } = "";
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is EatingHabit)) return false;
			return Id == ((EatingHabit)obj).Id;
		}
		public virtual ICollection<User> Users { get; set; } = [];
	}
}

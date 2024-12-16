using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
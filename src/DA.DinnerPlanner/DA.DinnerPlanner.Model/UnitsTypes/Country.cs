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
	public class Country : BaseModel
	{
		public string CountryName { get; set; } = "";
		/// <summary>
		/// ISO-3166-1
		/// </summary>
		/// <example>de-DE</example>
		public string CountryCode { get; set; } = "";
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Country)) return false;
			return Id.Equals(((Country)obj).Id);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DinnerPlanner.Model.GeoCode
{
	/// <ChangeLog>
	/// <Create Datum="24.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class GeoLocation : BaseModel
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public GeoCodeResult GeoCodeResult { get; set; }
		public override void Delete()
		{
			Deleted= true;
		}
	}
}

using DA.DinnerPlanner.Model.Contracts;
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
	public class OsmGeoCoder : IGeoCoder
	{
		public Task<GeoLocation> Address2LocationAsync(Address address)
		{
			throw new NotImplementedException();
		}
	}
}
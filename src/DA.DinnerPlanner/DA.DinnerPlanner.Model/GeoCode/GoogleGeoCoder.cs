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
	public class GoogleGeoCoder : Contracts.IGeoCoder
	{
		public Task<GeoLocation> Address2LocationAsync(Address address)
		{
			throw new NotImplementedException();
		}
	}
}

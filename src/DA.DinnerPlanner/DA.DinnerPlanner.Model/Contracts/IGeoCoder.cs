using DA.DinnerPlanner.Model.GeoCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DinnerPlanner.Model.Contracts
{
    /// <ChangeLog>
        /// <Create Datum="24.03.2025" Entwickler="DA" />
        /// </ChangeLog>
    public interface IGeoCoder
    {
        Task<GeoLocation> Address2LocationAsync(Address address);
    }
}

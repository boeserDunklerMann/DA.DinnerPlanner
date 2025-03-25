namespace DA.DinnerPlanner.Model.GeoCode
{
    /// <ChangeLog>
    /// <Create Datum="24.03.2025" Entwickler="DA" />
    /// </ChangeLog>
    /// <summary>
    /// the result of address->geocode translation
    /// </summary>
    public enum GeoCodeResult
    {
        /// <summary>
        /// Location found
        /// </summary>
        OK = 1,
        /// <summary>
        /// location not found
        /// </summary>
        NotFound = 2,
        /// <summary>
        /// location not found due to an unspecified server error
        /// </summary>
        Error=3
    }
}

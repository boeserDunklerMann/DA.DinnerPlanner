namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	public class Country : BaseModel
	{
		public string CountryName { get; set; } = "";
		/// <summary>
		/// ISO-3166-1
		/// </summary>
		/// <example>de-DE</example>
		public string CountryCode { get; set; } = "";
		public virtual ICollection<Address> Addresses { get; set; } = [];
		public override void Delete()
		{
			if (Addresses.Count > 0)
				throw new Exceptions.DeleteReferenceException($"CountryId {Id} is in use by {nameof(Address)}");
			Deleted = true;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not Country) return false;
			return Id.Equals(((Country)obj).Id);
		}
	}
}
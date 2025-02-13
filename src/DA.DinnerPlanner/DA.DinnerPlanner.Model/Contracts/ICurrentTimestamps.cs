namespace DA.DinnerPlanner.Model.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="13.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public interface ICurrentTimestamps
	{
		/// <summary>
		/// Änderungsdatum
		/// </summary>
		DateTime? ChangeDate { get; set; }

		/// <summary>
		/// Erstelldatum
		/// </summary>
		DateTime CreationDate { get; set; }

	}
}
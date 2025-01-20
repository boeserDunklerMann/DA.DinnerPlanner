namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="19.12.2024" Entwickler="DA">NumberPersonsAllowed added (Jira-Nr. DPLAN-6)</Change>
	/// </ChangeLog>
	/// <summary>
	/// class for a (planned) dinner, containing the guests, cooks and host
	/// </summary>
	public class Dinner : BaseModel
	{
		public Dinner()
		{
		}

		public User Host { get; set; } = new();
		/// <summary>
		/// max. Anzahl der Gäste (incl. Host), die eingeladen werden dürfen
		/// </summary>
		public int NumberPersonsAllowed { get; set; }

		public ICollection<User> Cooks { get; set; } = [];
		public ICollection<User> Guests { get; set; } = [];
		public DateTime DinnerDate { get; set; }
		public string Dinnerdescription { get; set; } = "";
		public ICollection<DinnerReview> Reviews { get; set; } = [];
	}
}
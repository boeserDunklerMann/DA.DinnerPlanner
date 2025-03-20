namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="19.12.2024" Entwickler="DA">NumberPersonsAllowed added (Jira-Nr. DPLAN-6)</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">prop HasTakenDone added</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">added LazyLoading support (virtal) (Jira-Nr. DPLAN-63)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	/// <summary>
	/// class for a (planned) dinner, containing the guests, cooks and host
	/// </summary>
	public class Dinner : BaseModel
	{
		public Dinner()
		{
		}

		public virtual User Host { get; set; } = new();
		/// <summary>
		/// max. Anzahl der Gäste (incl. Host), die eingeladen werden dürfen
		/// </summary>
		public int NumberPersonsAllowed { get; set; }

		public virtual ICollection<User> Cooks { get; set; } = [];
		public virtual ICollection<User> Guests { get; set; } = [];
		public DateTime DinnerDate { get; set; }
		public string Dinnerdescription { get; set; } = "";
		public virtual ICollection<DinnerReview> Reviews { get; set; } = [];
		public bool HasTakenDone { get; set; }

		public override void Delete()
		{
			if (Cooks.Count > 0)
				throw new Exceptions.DeleteReferenceException($"DinnerId {Id}  is in use by {nameof(User)} as {nameof(Cooks)}");
			if (Guests.Count > 0)
				throw new Exceptions.DeleteReferenceException($"DinnerId {Id}  is in use by {nameof(User)} as {nameof(Guests)}");
			if (Reviews.Count > 0)
				throw new Exceptions.DeleteReferenceException($"DinnerId {Id}  is in use by {nameof(DinnerReview)}");
			Deleted = true;
		}
	}
}
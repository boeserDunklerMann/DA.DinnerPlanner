namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">added LazyLoading support (virtal) (Jira-Nr. DPLAN-63)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	/// <summary>
	/// An users pet. Maybe interesting for allergical purposes
	/// </summary>
	public class Pet : BaseModel
	{
		/// <summary>
		/// Not the name of the pet but its decription, like "Dog", "Cat" or "Rhinoceros"
		/// </summary>
		public string Name { get; set; } = "";

		public virtual ICollection<User> Users { get; set; } = [];

		public override void Delete()
		{
			if (Users.Count > 0)
				throw new Exceptions.DeleteReferenceException($"PetId {Id} is in use by {nameof(User)}");
			Deleted = true;
		}
	}
}
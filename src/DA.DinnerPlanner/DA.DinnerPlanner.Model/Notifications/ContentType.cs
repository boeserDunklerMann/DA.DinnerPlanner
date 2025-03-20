namespace DA.DinnerPlanner.Model.Notifications
{
	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// possible values: “You were invited”, “Don’t forget the dinner”-Reminder, Google-Maps location
	/// see DPLAN-68
	/// </summary>
	class ContentType : BaseModel
	{
		public string Name { get; set; } = "";
		/// <summary>
		/// Toggles (un)read state
		/// </summary>
		public bool Read {  get; set; }
		public override void Delete()
		{
			if (Notifications.Count > 0)
				throw new Exceptions.DeleteReferenceException($"{nameof(ContentType)}Id {Id} is in use by {nameof(Notification)}");
			Deleted = true;
		}
		public virtual ICollection<Notification> Notifications { get; set; } = [];
	}
}
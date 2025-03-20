namespace DA.DinnerPlanner.Model.Notifications
{
	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// possible values: Web, Mail, SMS
	/// see DPLAN-68
	/// </summary>
	public enum DeliveryType
	{
		Web,
		Mail,
		SMS,
	}

	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// possible values: “You were invited”, “Don’t forget the dinner”-Reminder, Google-Maps location
	/// see DPLAN-68
	/// </summary>
	public enum ContentType 
	{
		Invitation,
		Reminder,
		Location
	}
}
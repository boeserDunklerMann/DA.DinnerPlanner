namespace DA.DinnerPlanner.Model.Notifications
{
	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// possible values: Web, Mail, SMS
	/// see DPLAN-68
	/// </summary>
	class DeliveryType : BaseModel
	{
		public string Name { get; set; } = "";
		public override void Delete()
		{
			if (Notifications.Count > 0)
				throw new Exceptions.DeleteReferenceException($"{nameof(DeliveryType)}Id {Id} is in use by {nameof(Notification)}");
			Deleted = true;
		}
		public virtual ICollection<Notification> Notifications { get; set; } = [];
	}
}
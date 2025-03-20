namespace DA.DinnerPlanner.Model.Notifications
{
	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// the notification itself
	/// </summary>
	class Notification : BaseModel
	{
		public string Content { get; set; } = "";
		public virtual DeliveryType DeliveryType { get; set; } = new();
		public virtual ContentType ContentType { get; set; } = new();
		public override void Delete()
		{
			Deleted = true;
		}
	}
}

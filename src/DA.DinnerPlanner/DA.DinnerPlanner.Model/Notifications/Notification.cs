namespace DA.DinnerPlanner.Model.Notifications
{
	/// <ChangeLog>
	/// <Create Datum="20.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// the notification itself
	/// </summary>
	public class Notification : BaseModel
	{
		public string Content { get; set; } = "";
		public DeliveryType DeliveryType { get; set; }
		public ContentType ContentType { get; set; }
		public virtual User User { get; set; } = new();
		public override void Delete()
		{
			Deleted = true;
		}
	}
}

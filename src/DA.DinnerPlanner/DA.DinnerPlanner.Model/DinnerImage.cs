namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="20.01.2025" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	public class DinnerImage : BaseModel
	{
		public byte[] Image { get; set; } = [];

		public override void Delete()
		{
			Deleted = true;
		}
	}
}
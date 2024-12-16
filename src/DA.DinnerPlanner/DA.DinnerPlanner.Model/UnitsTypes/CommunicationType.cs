namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	/// <example>
	/// email, phone, mobile, ...
	/// </example>
	public class CommunicationType : BaseModel
	{
		public string Name { get; set; } = "";
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is CommunicationType)) return false;
			return Id == ((CommunicationType)obj).Id;
		}
	}
}

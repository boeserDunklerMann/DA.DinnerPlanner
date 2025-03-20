namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	/// <example>
	/// email, phone, mobile, ...
	/// </example>
	public class CommunicationType : BaseModel
	{
		public string Name { get; set; } = "";
		public virtual ICollection<Communication> Communications { get; set; } = [];
		public override void Delete()
		{
			if (Communications.Count > 0)
				throw new Exceptions.DeleteReferenceException($"CommunicationTypeId {Id} is in use by {nameof(Communication)}");
			Deleted = true;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null || obj is not CommunicationType) return false;
			return Id == ((CommunicationType)obj).Id;
		}
	}
}

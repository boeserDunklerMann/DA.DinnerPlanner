﻿using DA.DinnerPlanner.Model.UnitsTypes;
using System.Text.Json.Serialization;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class Communication : BaseModel
	{
		public CommunicationType CommunicationType { get; set; }=new CommunicationType();
		/// <summary>
		/// the communication value itself, like mailaddress, mobilenumber, ...
		/// </summary>
		public string CommunicationValue { get; set; } = "";
		[JsonIgnore]
		public virtual User User { get; set; }
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Communication)) return false;
			return Id == ((Communication)obj).Id;
		}
	}
}

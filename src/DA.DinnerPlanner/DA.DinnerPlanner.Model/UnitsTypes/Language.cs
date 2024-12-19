namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="19.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class Language : BaseModel
	{
		/*
Mandarin-Chinesisch
Spanisch
Englisch
Hindi
Arabisch
Bengalisch
Portugiesisch
Russisch
Japanisch
Punjabi
Deutsch
Französisch
Türkisch
Koreanisch
Italienisch
Swahili
Urdu
Thai
Vietnamesisch
Tagalog (Filipino)
Griechisch
		 */
		public string Name { get; set; } = "";
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Language)) return false;
			return Id == ((BaseModel)obj).Id;
		}
		public ICollection<User> Users { get; set; } = [];

	}
}
namespace DA.DinnerPlanner.Model.UnitsTypes
{
	/// <ChangeLog>
	/// <Create Datum="19.12.2024" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
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
			if (obj == null || obj is not Language) return false;
			return Id == ((BaseModel)obj).Id;
		}

		public override void Delete()
		{
			if (Users.Count > 0)
				throw new Exceptions.DeleteReferenceException($"LanguageId {Id} is in use by {nameof(User)}");
		}

		public virtual ICollection<User> Users { get; set; } = [];

	}
}
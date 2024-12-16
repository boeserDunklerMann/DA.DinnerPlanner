namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// class for a (planned) dinner, containing the guests, cooks and host
	/// </summary>
	public class Dinner : BaseModel
	{
		public Dinner()
		{
		}

		public User Host { get; set; } = new();

		public ICollection<User> Cooks { get; set; } = [];
		public ICollection<User> Guests { get; set; } = [];
		public DateTime DinnerDate { get; set; }
		public string Dinnerdescription { get; set; }
	}
}
namespace DA.DinnerPlanner.Model.Auth
{
	/// <ChangeLog>
	/// <Create Datum="27.01.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class GoogleUser
	{
		public Guid Id { get; set; }
		public string Token { get; set; } = "";
		public DateTime? Stamp { get; set; }
		public string? FullName { get; set; }
		public string? ImgUrl { get; set; }
		public string? EMail { get; set; }
	}
}

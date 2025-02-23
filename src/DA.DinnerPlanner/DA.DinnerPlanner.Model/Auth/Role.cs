using System.ComponentModel.DataAnnotations.Schema;

namespace DA.DinnerPlanner.Model.Auth
{
	/// <ChangeLog>
	/// <Create Datum="23.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	[Table("Auth_" + nameof(Role))]
	public class Role : BaseModel
	{
		public string Name { get; set; } = "";

		public ICollection<User> Users { get; set; } = [];
	}
}
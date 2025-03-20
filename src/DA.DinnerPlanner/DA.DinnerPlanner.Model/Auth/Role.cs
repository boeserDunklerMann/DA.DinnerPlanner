using System.ComponentModel.DataAnnotations.Schema;

namespace DA.DinnerPlanner.Model.Auth
{
	/// <ChangeLog>
	/// <Create Datum="23.02.2025" Entwickler="DA" />
	/// <Change Datum="20.03.2025" Entwickler="DA">added LazyLoading support (virtal) (Jira-Nr. DPLAN-63)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	[Table("Auth_" + nameof(Role))]
	public class Role : BaseModel
	{
		public string Name { get; set; } = "";

		public virtual ICollection<User> Users { get; set; } = [];

		public override void Delete()
		{
			if (Users.Count > 0)
				throw new Exceptions.DeleteReferenceException($"RoleId {Id} is in use by {nameof(User)}");
			Deleted = true;
		}
	}
}
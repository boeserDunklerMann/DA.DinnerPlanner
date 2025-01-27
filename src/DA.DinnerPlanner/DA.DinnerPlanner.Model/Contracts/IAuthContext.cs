using DA.DinnerPlanner.Model.Auth;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Model.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="27.01.2025" Entwickler="DA" />
	/// </ChangeLog>
	public interface IAuthContext
	{
		string ConnectionString { get; set; }
		DbSet<GoogleUser> GoogleUsers { get; set; }

		Task SaveAsync();
	}
}
using DA.DinnerPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="27.01.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class AuthContext : DbContext, IAuthContext
	{
		public DbSet<Auth.GoogleUser> GoogleUsers { get; set; }
		public string ConnectionString { get; set; } = "";
		public async Task SaveAsync()
		{
			await SaveChangesAsync();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// https://stackoverflow.com/questions/74060289/mysqlconnection-open-system-invalidcastexception-object-cannot-be-cast-from-d
			// MariaDB 11+ doesnt work because of nullable PKs?
			optionsBuilder.UseMySQL(ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Auth.GoogleUser>(guser =>
			{
				guser.HasKey(gu => gu.Id);
			});
		}
	}
}

using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	public class DinnerPlannerContext(string connectionString):DbContext
	{
		public DbSet<Country> Countries { get; set; }
		public DbSet<CommunicationType> CommunicationTypes { get; set; }
		public DbSet<Allergy> Allergies { get; set; }
		public DbSet<User> Users { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// https://stackoverflow.com/questions/74060289/mysqlconnection-open-system-invalidcastexception-object-cannot-be-cast-from-d
			// MariaDB 11+ doesnt work because of nullable PKs?
			optionsBuilder.UseMySQL(connectionString);	// MariaDB10
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// TODO DA: add/modify entity-definitions here
			// https://learn.microsoft.com/de-de/ef/core/modeling/relationships/one-to-many
			// https://learn.microsoft.com/de-de/ef/core/modeling/relationships/many-to-many <--- das ist das Richtige!
			modelBuilder.Entity<User>(user =>
			{
				user.HasKey(u => u.Id);
				user.HasMany(u => u.Allergies);
				user.HasMany(u => u.AddressList).WithOne(a => a.User);
				user.HasMany(u=>u.CommunicationList).WithOne(c => c.User);
				user.HasAlternateKey(u => u.GoogleId);
			});
			modelBuilder.Entity<Allergy>(allergy =>
			{
				allergy.HasKey(a => a.Id);

			});
			modelBuilder.Entity<Communication>(communication =>
			{
				communication.HasKey(c => c.Id);
				communication.HasOne(c => c.CommunicationType);
			});
			modelBuilder.Entity<Address>(address =>
			{
				address.HasKey(a => a.Id);
				address.HasOne(a => a.Country);
			});
		}
	}
}
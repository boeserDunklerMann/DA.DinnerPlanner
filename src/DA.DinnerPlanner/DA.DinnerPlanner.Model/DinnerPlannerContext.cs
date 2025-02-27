using DA.DinnerPlanner.Model.Auth;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="19.12.2024" Entwickler="DA">EatingHabits added (Jira-Nr. DPLAN-4)</Change>
	/// <Change Datum="19.12.2024" Entwickler="DA">Languages added (Jira-Nr. DPLAN-8)</Change>
	/// <Change Datum="20.01.2025" Entwickler="DA">ConnectionString SaveAsync added (Jira-Nr. DPLAN-23)</Change>
	/// <Change Datum="27.01.2025" Entwickler="DA">GoogleUsers added (Jira-Nr. DPLAN-38)</Change>
	/// <Change Datum="13.02.2025" Entwickler="DA">SaveChangesAsync overridden (Jira-Nr. DPLAN-41)</Change>
	/// <Change Datum="23.02.2025" Entwickler="DA">Roles added (Jira-Nr. DPLAN-44)</Change>
	/// <Change Datum="23.02.2025" Entwickler="DA">UpdateChangeDates added (Jira-Nr. DPLAN-41)</Change>
	/// <Change Datum="27.02.2025" Entwickler="DA">IConfiguration added</Change>
	/// </ChangeLog>
	public class DinnerPlannerContext : DbContext, IDinnerPlannerContext
	{
		#region Private fields
		private readonly IConfiguration? configuration;
		#endregion
		#region DBSets
		public DbSet<Country> Countries { get; set; }
		public DbSet<CommunicationType> CommunicationTypes { get; set; }
		public DbSet<Allergy> Allergies { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Dinner> Dinners { get; set; }
		public DbSet<Pet> Pets { get; set; }
		public DbContext Db => this;
		public DbSet<EatingHabit> EatingHabits { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<UserImage> UserImages { get; set; }
		public DbSet<DinnerImage> DinnerImages { get; set; }
		public DbSet<GoogleUser> GoogleUsers { get; set; }
		public DbSet<Role> Roles { get; set; }
		public string ConnectionString { get; set; } = "";
		#endregion

		public DinnerPlannerContext(IConfiguration? cfg = null) : base()
		{
			configuration = cfg;
			if (configuration != null)
				ConnectionString = configuration["ConnectionStrings:da_dinnerplanner - db"]!;
		}

		public async Task SaveAsync()
		{
			await SaveChangesAsync();
		}

		public override int SaveChanges()
		{
			UpdateChangeDates();
			return base.SaveChanges();
		}
		public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			UpdateChangeDates();
			return await base.SaveChangesAsync(cancellationToken);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// https://stackoverflow.com/questions/74060289/mysqlconnection-open-system-invalidcastexception-object-cannot-be-cast-from-d
			// MariaDB 11+ doesnt work because of nullable PKs?
			optionsBuilder.UseMySQL(ConnectionString);  // MariaDB10
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// https://learn.microsoft.com/de-de/ef/core/modeling/relationships/one-to-many
			// https://learn.microsoft.com/de-de/ef/core/modeling/relationships/many-to-many <--- das ist das Richtige!
			modelBuilder.Entity<User>(user =>
			{
				user.HasKey(u => u.Id);
				user.HasMany(u => u.Allergies);
				user.HasMany(u => u.AddressList).WithOne(a => a.User);
				user.HasMany(u => u.CommunicationList).WithOne(c => c.User);
				user.HasMany(u => u.Pets);
				user.Property(u => u.GoogleId).IsRequired(false);
				user.HasIndex(u => u.GoogleId).HasFilter("LABEL IS NOT NULL").IsUnique();
			});
			modelBuilder.Entity<Role>(role =>
			{
				role.ToTable($"Auth_{nameof(Role)}");
				role.HasKey(r => r.Id);
			});

			modelBuilder.Entity<Allergy>(allergy =>
			{
				allergy.HasKey(a => a.Id);

			});
			modelBuilder.Entity<EatingHabit>(eh =>
			{
				eh.HasKey(e => e.Id);
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
			modelBuilder.Entity<Dinner>(dinner =>
			{
				dinner.HasKey(d => d.Id);
				dinner.HasMany(d => d.Reviews).WithOne(r => r.Dinner);
				dinner.HasOne(d => d.Host).WithMany(u => u.DinnerAsHost);
				dinner.HasMany(d => d.Cooks).WithMany(u => u.DinnerAsCook).UsingEntity("DinnerCook");
				dinner.HasMany(d => d.Guests).WithMany(u => u.DinnerAsGuest).UsingEntity("DinnerGuest");
			});
			modelBuilder.Entity<DinnerReview>(dr =>
			{
				dr.HasKey(d => d.Id);
				dr.HasOne(d => d.ReviewsAuthor).WithMany(u => u.Reviews);
				dr.HasOne(d => d.Dinner).WithMany(d => d.Reviews);
			});
			modelBuilder.Entity<UserImage>(image =>
			{
				image.HasKey(i => i.Id);
				image.HasOne(i => i.User).WithMany(u => u.UserImages);
			});
			modelBuilder.Entity<Auth.GoogleUser>(guser =>
			{
				guser.HasKey(gu => gu.Id);
			});
		}
		private void UpdateChangeDates()
		{
			DateTime now = DateTime.UtcNow;
			var createdEntries = ChangeTracker.Entries()
				.Where(entry => entry.State == EntityState.Added).ToList();
			createdEntries.ForEach(e =>
			{
				var prop = e.Properties.FirstOrDefault(prop => prop.Metadata.Name.Equals(nameof(ICurrentTimestamps.CreationDate)));
				if (prop != null)
					prop.CurrentValue = now;
			});

			var changedEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Modified).ToList();
			changedEntries.ForEach(e =>
			{
				var prop = e.Properties.FirstOrDefault(prop => prop.Metadata.Name.Equals(nameof(ICurrentTimestamps.ChangeDate)));
				if (prop != null)
					prop.CurrentValue = now;
			});
		}
	}
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DA.DinnerPlanner.Common
{
	/// <ChangeLog>
	/// <Create Datum="20.12.2024" Entwickler="DA" />
	/// <Change Datum="21.12.2024" Entwickler="DA">class made singleton (Jira-Nr. DPLAN-20)</Change>
	/// https://csharpindepth.com/articles/singleton
	/// </ChangeLog>
	public sealed class Application
	{
		private static readonly Lazy<Application> lazy = new(() => new Application());

		private Lazy<Model.DinnerPlannerContext>? context;
		public static Application Instance =>lazy.Value;

		public Application()
		{
			context = null;
		}
		public void SetConfig(IConfiguration config)
		{
			string? connString = config["ConnectionStrings:da_dinnerplanner-db"];
			if (!string.IsNullOrEmpty(connString))
				context = new Lazy<Model.DinnerPlannerContext>(() => new Model.DinnerPlannerContext(connString));
		}
		public async Task<ICollection<Model.User>> GetAllUsersAsync()
		{
			if (context== null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			return await context.Value.Users
					.Include(nameof(Model.User.AddressList))
					.Include(nameof(Model.User.Allergies))
					.Include(nameof(Model.User.CommunicationList))
					.Include(nameof(Model.User.DinnerAsCook))
					.Include(nameof(Model.User.DinnerAsGuest))
					.Include(nameof(Model.User.DinnerAsHost))
					.Include(nameof(Model.User.Reviews))
					.Include(nameof(Model.User.UserImages))
					.Include(nameof(Model.User.Languages))
					.Where(u => !u.Deleted).ToListAsync();
		}

		public async Task<ICollection<Model.Dinner>> GetAllDinnersAsync()
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			return await context.Value.Dinners
				.Where(d => !d.Deleted).ToListAsync();
		}

		/// <summary>
		/// adds a new dinner to the ctx and saves it to db
		/// </summary>
		/// <param name="newDinner">the new dinner</param>
		public async Task CreateDinnerAsync(Model.Dinner newDinner)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			if (newDinner != null)
			{
				context.Value.Dinners.Add(newDinner);
				await context.Value.Db.SaveChangesAsync();
			}
		}

		public async Task CreateUserAsync(Model.User newUser)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			if (newUser!=null)
			{
				await context.Value.Users.AddAsync(newUser);
				await context.Value.Db.SaveChangesAsync();
			}
		}
	}
}

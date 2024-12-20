using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DinnerPlanner.Common
{
	/// <ChangeLog>
		/// <Create Datum="20.12.2024" Entwickler="DA" />
		/// </ChangeLog>
	public class Application
	{
		private readonly Lazy<Model.DinnerPlannerContext> context;
		public Application(IConfiguration config)
		{
			string? connString = config["ConnectionStrings:da_dinnerplanner-db"];
			if (!string.IsNullOrEmpty(connString))
				context = new Lazy<Model.DinnerPlannerContext>(() => new Model.DinnerPlannerContext(connString));
		}
		public async Task<ICollection<Model.User>> GetAllUsersAsync()
		{
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
			return await context.Value.Dinners
				.Where(d => !d.Deleted).ToListAsync();
		}

		/// <summary>
		/// adds a new dinner to the ctx and saves it to db
		/// </summary>
		/// <param name="newDinner">the new dinner</param>
		public async Task CreateDinnerAsync(Model.Dinner newDinner)
		{
			if (newDinner != null)
			{
				context.Value.Dinners.Add(newDinner);
				await context.Value.Db.SaveChangesAsync();
			}
		}

		public async Task CreateUserAsync(Model.User newUser)
		{
			if (newUser!=null)
			{
				await context.Value.Users.AddAsync(newUser);
				await context.Value.Db.SaveChangesAsync();
			}
		}
	}
}

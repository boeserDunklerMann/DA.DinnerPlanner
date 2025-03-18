using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Configuration;

namespace DA.DinnerPlanner.Common
{
	/// <ChangeLog>
	/// <Create Datum="20.12.2024" Entwickler="DA" />
	/// <Change Datum="21.12.2024" Entwickler="DA">class made singleton (Jira-Nr. DPLAN-20)</Change>
	/// https://csharpindepth.com/articles/singleton
	/// <Change Datum="20.01.2025" Entwickler="DA">context removed, we get it via DI (Jira-Nr. DPLAN-23)</Change>
	/// <Change Datum="13.02.2025" Entwickler="DA">dont load deleted objects (Jira-Nr. DPLAN-42)</Change>
	/// <Change Datum="23.02.2025" Entwickler="DA">CalculateDinerAsync added (Jira-Nr. DPLAN-45)</Change>
	/// <Change Datum="18.03.2025" Entwickler="DA">GetUserByIdAsync added</Change>
	/// </ChangeLog>
	public sealed class Application
	{
		private static readonly Lazy<Application> lazy = new(() => new Application());

		public static Application Instance => lazy.Value;

		public Application()
		{
		}

		#region DB stuff
		public async Task<ICollection<Model.User>> GetAllUsersAsync(IDinnerPlannerContext context)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			return await context.Users
					.Include(u=>u.AddressList.Where(a=>!a.Deleted))
					.Include(user=>user.Allergies.Where(allergy=>!allergy.Deleted))
					.Include(user=>user.CommunicationList.Where(comm=>!comm.Deleted))
						.ThenInclude(cl => cl.CommunicationType)
					.Include(nameof(Model.User.DinnerAsCook))
					.Include(nameof(Model.User.DinnerAsGuest))
					.Include(nameof(Model.User.DinnerAsHost))
					.Include(nameof(Model.User.Reviews))
					.Include(u=>u.UserImages.Where(img=>!img.Deleted))
					.Include(nameof(Model.User.Languages))
					.Include(nameof(Model.User.Pets))
					.Include(nameof(Model.User.EatingHabit))
					.Where(u => !u.Deleted).ToListAsync();
		}

		public async Task<User> GetUserByIdAsync(IDinnerPlannerContext context, int userId)
		{
			User retval = (await GetAllUsersAsync(context)).Single(u => u.Id == userId);
			if (retval == null)
				throw new Exception($"User not found Id: {userId}");    // TODO DA: create own Exceptionclass for this
			return retval;
		}
		public async Task<ICollection<Model.Dinner>> GetAllDinnersAsync(IDinnerPlannerContext context)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			return await context.Dinners
				.Where(d => !d.Deleted).ToListAsync();
		}

		/// <summary>
		/// adds a new dinner to the ctx and saves it to db
		/// </summary>
		/// <param name="newDinner">the new dinner</param>
		public async Task CreateDinnerAsync(IDinnerPlannerContext context, Model.Dinner newDinner)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			if (newDinner != null)
			{
				context.Dinners.Add(newDinner);
				await context.Db.SaveChangesAsync();
			}
		}

		public async Task CreateUserAsync(IDinnerPlannerContext context, Model.User newUser)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			if (newUser != null)
			{
				await context.Users.AddAsync(newUser);
				await context.Db.SaveChangesAsync();
			}
		}
		#endregion

		#region Applogic stuff
		/// <summary>
		/// "Calculates" guests for incoming dinner, in dependence of languages and (pets and allergies)
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dinner2Calculate"></param>
		/// <returns></returns>
		public async Task<Dinner> CalculateDinerAsync(IDinnerPlannerContext context, Dinner dinner2Calculate)
		{
			// 1. Find users who talks the sam languages as the host
			var languages = dinner2Calculate.Host.Languages;
			var possibleUsers=context.Users.Where(u => u.Languages.Any(l => languages.Contains(l)));
			return await Task.FromResult(dinner2Calculate);
		}
		#endregion
	}
}
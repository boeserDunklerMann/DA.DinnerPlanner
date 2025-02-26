﻿using DA.DinnerPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DA.DinnerPlanner.Common
{
	/// <ChangeLog>
	/// <Create Datum="20.12.2024" Entwickler="DA" />
	/// <Change Datum="21.12.2024" Entwickler="DA">class made singleton (Jira-Nr. DPLAN-20)</Change>
	/// https://csharpindepth.com/articles/singleton
	/// <Change Datum="20.01.2025" Entwickler="DA">context removed, we get it via DI (Jira-Nr. DPLAN-23)</Change>
	/// <Change Datum="13.02.2025" Entwickler="DA">dont load deleted objects (Jira-Nr. DPLAN-42)</Change>
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

		#endregion
	}
}
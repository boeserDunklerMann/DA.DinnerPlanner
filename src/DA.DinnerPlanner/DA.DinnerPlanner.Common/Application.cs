using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Immutable;

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
	/// <Change Datum="18.03.2025" Entwickler="DA">added Reviews in GetAllDinnersAsync (Jira-Nr. DPLAN-64)</Change>
	/// <Change Datum="18.03.2025" Entwickler="DA">GetDinnerByIdAsync added (Jira-Nr. DPLAN-65)</Change>
	/// <Change Datum="25.03.2025" Entwickler="DA">InviteGuests4Dinner added (Jira-Nr. DPLAN-66)</Change>
	/// </ChangeLog>
	public sealed class Application
	{
		private static readonly Lazy<Application> lazy = new(() => new Application());

		public static Application Instance => lazy.Value;
		private Random random = new Random(DateTime.Now.Millisecond);
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
					.Include(u => u.AddressList.Where(a => !a.Deleted))
					.Include(user => user.Allergies.Where(allergy => !allergy.Deleted))
					.Include(user => user.CommunicationList.Where(comm => !comm.Deleted))
						.ThenInclude(cl => cl.CommunicationType)
					.Include(nameof(Model.User.DinnerAsCook))
					.Include(nameof(Model.User.DinnerAsGuest))
					.Include(nameof(Model.User.DinnerAsHost))
					.Include(nameof(Model.User.Reviews))
					.Include(u => u.UserImages.Where(img => !img.Deleted))
					.Include(nameof(Model.User.Languages))
					.Include(nameof(Model.User.Pets))
					.Include(nameof(Model.User.EatingHabit))
					.Where(u => !u.Deleted).ToListAsync();
		}

		public async Task<User> GetUserByIdAsync(IDinnerPlannerContext context, int userId)
		{
			User retval = (await GetAllUsersAsync(context)).Single(u => u.Id == userId);
			if (retval == null)
				throw new Exception($"User not found Id: {userId}");    // DONE DA: create own Exceptionclass for this -> .Single will throw exc
			return retval;
		}
		public async Task<ICollection<Model.Dinner>> GetAllDinnersAsync(IDinnerPlannerContext context)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			return await context.Dinners
				.Include(nameof(Dinner.Reviews))
				.Include(nameof(Dinner.Host))
				.Where(d => !d.Deleted).ToListAsync();
		}
		public async Task<Dinner> GetDinnerByIdAsync(IDinnerPlannerContext context, int dinnerId)
		{
			if (context == null)
			{
				throw new NullReferenceException($"{nameof(context)} not set.");
			}
			return await context.Dinners
				.Include(nameof(Dinner.Reviews))
				.Include(nameof(Dinner.Host))
				.FirstAsync(d => !d.Deleted && d.Id == dinnerId);
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
			var possibleUsers = context.Users.Where(u => u.Languages.Any(l => languages.Contains(l) &&!l.Deleted) && u.Id != dinner2Calculate.Host.Id);
			Address hostsAddress = dinner2Calculate.Host.AddressList.First(a => a.Primary &&!a.Deleted);
			List<User> usersInTown = new();
			// 2. Find users who live in the same city as the host
			possibleUsers.ToList().ForEach(u =>
			{
				if (u.AddressList.Where(addr=>!addr.Deleted).Select(a => a.City).Contains(hostsAddress.City))
				{
					usersInTown.Add(u);
				}
			});
			var mixedUsers = usersInTown.OrderBy(_ => Guid.NewGuid());
			int skipper = random.Next(0, mixedUsers.Count());
			var guests = mixedUsers.Skip(skipper).Take(dinner2Calculate.NumberPersonsAllowed - 1);
			dinner2Calculate.Guests = guests.ToList();
			await context.SaveAsync();
			return dinner2Calculate;
		}

		public async Task InviteGuests4Dinner(IConfiguration cfg, IDinnerPlannerContext context, Dinner dinner)
		{
			// send invitations to guests
			dinner.Guests.ToImmutableList().ForEach(u =>
			{
				context.Notifications.Add(new()
				{
					Content = cfg["Notification:GuestInvitationTemplate"]!,
					ContentType = ContentType.Invitation,
					DeliveryType = DeliveryType.Web,
					User = u
				});
			});
			// send invitations to cooks
			dinner.Cooks.ToImmutableList().ForEach(u =>
			{
				context.Notifications.Add(new()
				{
					Content = cfg["Notification:CookInvitationTemplate"]!,
					ContentType = ContentType.Invitation,
					DeliveryType = DeliveryType.Web,
					User = u
				});
			});
			await context.SaveAsync();
		}
		#endregion
	}
}
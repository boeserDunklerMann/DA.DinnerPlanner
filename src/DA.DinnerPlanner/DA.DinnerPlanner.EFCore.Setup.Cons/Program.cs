using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.UnitsTypes;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.EFCore.Setup.Cons
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// </ChangeLog>
	/// <see href="https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-example.html"/>
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("database will be dropped!!");
			Console.WriteLine("Press any key or CTRL+C");
			Console.ReadKey();
			InsertData();
			ReadData();
		}

		static void InsertData()
		{
			using DinnerPlannerContext ctx = new("Server=192.168.2.108;Database=DinnerPlanner_dev;Uid=root;Pwd=only4sus");
			ctx.Database.EnsureDeleted();
			ctx.Database.EnsureCreated();

			// add countries
			Country country = new Country { CountryName = "Deutschland", CountryCode = "DE" };
			ctx.Countries.Add(country);
			ctx.Countries.AddRange([
				new Country{CountryName="Griechenland", CountryCode="GR"},
				new Country{CountryName="Österreich", CountryCode="AT"},
				new Country{CountryName="Polen", CountryCode="PL"}
				]);
			//ctx.SaveChanges();

			// add commtypes
			ctx.CommunicationTypes.AddRange([
				new CommunicationType{Name="E-Mail"},
				new CommunicationType{Name="Mobile"},
				new CommunicationType{Name="Phone"},
				new CommunicationType{Name="Instagram"}
				]);
			//ctx.SaveChanges();

			// add allergies
			ctx.Allergies.AddRange([
				new Allergy{Name="Katzenhaare"},
				new Allergy{Name="Hundehaare"},

				new Allergy{Name="Nüsse"},
				new Allergy{Name="Sellerie"},
				new Allergy{Name="Gluten"}
				]);

			ctx.Pets.AddRange([
				new Pet{Name="Katze"},
				new Pet{Name="19jähriges Ungeheuer"}
				]);
			ctx.SaveChanges();

			User andre = new()
			{
				FirstName = "André",
				LastName = "Dunkel",
				BirthDate = new DateTime(1978, 7, 21),
				GoogleId = "102882888849269001816",
				UsersComment="Hab ne große Wohnung aber kann nicht kochen!"
			};
			andre.AddressList.Add(new Address { Street = "Max-Liebermann-Straße", HouseNumber = "180", City = "Leipzig", Country = ctx.Countries.First(c => c.Id == 1) });
			andre.Allergies.Add(ctx.Allergies.First(a => a.Id == 2));
			andre.Allergies.Add(ctx.Allergies.First(a => a.Id == 4));
			andre.Pets.Add(ctx.Pets.First(p=>p.Id ==1));
			andre.CommunicationList.Add(new Communication { CommunicationValue = "andre.dunkel@provider.de", CommunicationType = ctx.CommunicationTypes.First(ct => ct.Id == 1) });
			andre.CommunicationList.Add(new Communication { CommunicationValue = "+493411234", CommunicationType = ctx.CommunicationTypes.First(ct => ct.Id == 3) });
			ctx.Users.Add(andre);

			User eleni = new()
			{
				FirstName = "Eleni",
				LastName = "Kalperi",
				BirthDate = new DateTime(1985, 2, 6)
			};
			eleni.Allergies.Add(ctx.Allergies.First(a=>a.Id == 2));
			eleni.Allergies.Add(ctx.Allergies.First(a=>a.Id == 4));
			eleni.Pets.Add(ctx.Pets.First(p => p.Id == 2));
			eleni.CommunicationList.Add(new Communication { CommunicationValue = "+493411234", CommunicationType = ctx.CommunicationTypes.First(ct => ct.Id == 3) });
			ctx.Users.Add(eleni);

			User chrysoula = new()
			{
				FirstName = "Chrysoula",
				LastName = "Tzirimi",
				BirthDate = new DateTime(2005, 09, 25),
				UsersComment="Kann kochen, hab aber keine Wohnung"
			};
			chrysoula.CommunicationList.Add(new Communication { CommunicationValue = "chrysoula_tzirimi", CommunicationType = ctx.CommunicationTypes.First(ct => ct.Id == 4) });
			ctx.Users.Add(chrysoula);
			ctx.SaveChanges();

			Dinner dinner = new()
			{
				DinnerDate = new DateTime(2024, 12, 31, 18, 00, 0),
				Dinnerdescription = "Silvesterdinner",
				Host = andre
			};
			dinner.Cooks.Add(chrysoula);
			dinner.Guests.Add(chrysoula);
			dinner.Guests.Add(eleni);
			ctx.Dinners.Add(dinner);
			ctx.SaveChanges();

			DinnerReview review = new()
			{
				NumberStars4Dinner = 5,
				ReviewDinner = "War ein sehr schöner Abend",
				NumberStars4Cook = 5,
				ReviewCook = "Essen war sehr lecker",
				NumberStars4Host = 0,
				ReviewHost = "Klo war dreckig",
				ReviewsAuthor=eleni
			};
			dinner.Reviews.Add(review);
			ctx.SaveChanges();
		}

		static void ReadData()
		{
			using DinnerPlannerContext ctx = new("Server=192.168.2.108;Database=DinnerPlanner_dev;Uid=root;Pwd=only4sus");
			{
				var countries = ctx.Countries.ToList();
				var ctypes = ctx.CommunicationTypes.ToList();
				var users = ctx.Users
					.Include("AddressList")
					.Include("Allergies")
					.Include("CommunicationList")
					.Include(nameof(User.DinnerAsCook))
					.Include(nameof(User.DinnerAsGuest))
					.Include(nameof(User.DinnerAsHost))
					.Include(nameof(User.Reviews))
				.ToList();
				users.ForEach(u =>
				{
					Console.WriteLine(u.GetDefaultDisplayName());
					Console.WriteLine("Addr:");
					u.AddressList.ToList().ForEach(a => Console.WriteLine($"\t{a.Street} {a.HouseNumber} {a.City} {a.Country.CountryCode}"));
					Console.WriteLine("Komm:");
					u.CommunicationList.ToList().ForEach(c => Console.WriteLine($"\t{c.CommunicationType.Name}: {c.CommunicationValue}"));
					Console.WriteLine("Allergien:");
					u.Allergies.ToList().ForEach(a =>
					{
						Console.WriteLine($"\t{a.Name}");
					});
				});
			}
		}
	}
}

using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.UnitsTypes;

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
			Console.WriteLine("Drop database before running");
			Console.WriteLine("Press any key");
			Console.ReadKey();
			InsertData();
		}

		static void InsertData()
		{
			using DinnerPlannerContext ctx = new("Server=127.0.0.1;Database=DinnerPlanner_dev;Uid=root;Pwd=****");
			ctx.Database.EnsureCreated();

			// add countries
			Country country = new Country { CountryName = "Deutschland", CountryCode = "DE" };
			ctx.Countries.Add(country);
			ctx.Countries.AddRange([
				new Country{CountryName="Griechenland", CountryCode="GR"},
				new Country{CountryName="Österreich", CountryCode="AT"},
				new Country{CountryName="Polen", CountryCode="PL"}
				]);
			ctx.SaveChanges();

			// add commtypes
			ctx.CommunicationTypes.AddRange([
				new CommunicationType{Name="E-Mail"},
				new CommunicationType{Name="Mobile"},
				new CommunicationType{Name="Phone"}
				]);
			ctx.SaveChanges();
		}
	}
}

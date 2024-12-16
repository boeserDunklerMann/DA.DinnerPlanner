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
	/// <summary>
	/// An users pet. Maybe interesting for allergical purposes
	/// </summary>
	public class Pet : BaseModel
	{
		/// <summary>
		/// Not the name of the pet but its decription, like "Dog", "Cat" or "Rhinoceros"
		/// </summary>
		public string Name { get; set; } = "";

		public ICollection<User> Users { get; set; } = [];
	}
}
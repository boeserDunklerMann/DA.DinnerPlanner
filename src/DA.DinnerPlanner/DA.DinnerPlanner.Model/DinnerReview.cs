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
	/// <summary>a review for the dinner itself, the host, the cooks</summary>
	public class DinnerReview : BaseModel
	{
		public DinnerReview() { }
		public int NumberStars4Dinner { get; set; }
		/// <summary>
		/// reviewtext for the dinner itself
		/// </summary>
		public string ReviewDinner { get; set; } = "";
		public int NumberStars4Host { get; set; }
		public string ReviewHost { get; set; } = "";
		public int NumberStars4Cook { get; set; }
		public string ReviewCook { get; set; } = "";
		/// <summary>
		/// Who wrote the review
		/// </summary>
		public User ReviewsAuthor { get; set; } = new();
		public Dinner Dinner { get; set; } = new();
	}
}
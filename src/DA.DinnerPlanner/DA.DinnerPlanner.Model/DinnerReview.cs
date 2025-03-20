using System.Net;

namespace DA.DinnerPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="16.12.2024" Entwickler="DA" />
	/// <Change Datum="20.01.2025" Entwickler="DA">DinnerImages added</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">added LazyLoading support (virtal) (Jira-Nr. DPLAN-63)</Change>
	/// <Change Datum="20.03.2025" Entwickler="DA">method Delete added (Jira-Nr. DPLAN-60)</Change>
	/// </ChangeLog>
	/// <summary>a review for the dinner itself, the host, the cooks</summary>
	public class DinnerReview : BaseModel
	{
		public DinnerReview() { }
		/// <summary>
		/// Rating for the evening itself.
		/// </summary>
		public int NumberStars4Dinner { get; set; }
		/// <summary>
		/// reviewtext for the dinner itself
		/// </summary>
		public string ReviewDinner { get; set; } = "";
		/// <summary>
		/// Rating for the host in person
		/// </summary>
		public int NumberStars4Host { get; set; }
		public string ReviewHost { get; set; } = "";
		/// <summary>
		/// Rating for the cook/the meal.
		/// </summary>
		public int NumberStars4Cook { get; set; }
		public string ReviewCook { get; set; } = "";
		/// <summary>
		/// Who wrote the review
		/// </summary>
		public virtual User ReviewsAuthor { get; set; } = new();
		public virtual Dinner Dinner { get; set; } = new();
		public virtual ICollection<DinnerImage> DinnerImages { get; set; } = [];

		public override void Delete()
		{
			if (DinnerImages.Count >0)
				throw new Exceptions.DeleteReferenceException($"DinnerReviewId {Id} is in use by {nameof(DinnerImage)} as {nameof(DinnerImages)}");
		}
	}
}
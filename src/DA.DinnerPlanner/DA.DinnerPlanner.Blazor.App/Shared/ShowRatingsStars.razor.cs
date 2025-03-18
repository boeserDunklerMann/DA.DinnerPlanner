using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Shared
{
	/// <ChangeLog>
	/// <Create Datum="18.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class ShowRatingsStars : ComponentBase
	{
		[Parameter]
		public Dinner? Dinner { get; set; }
		private double rating = -1;	// -1 disabled, show no stars, because no reviews
		protected override async Task OnInitializedAsync()
		{
			if (Dinner != null && Dinner.Reviews.Count > 0)
			{
				double cook = Dinner!.Reviews.Average(r => r.NumberStars4Cook);
				double host = Dinner.Reviews.Average(r => r.NumberStars4Host);
				double dinner = Dinner.Reviews.Average(r => r.NumberStars4Dinner);
				rating = (cook + host + dinner) / 3;
			}
			await base.OnInitializedAsync();
		}
	}
}
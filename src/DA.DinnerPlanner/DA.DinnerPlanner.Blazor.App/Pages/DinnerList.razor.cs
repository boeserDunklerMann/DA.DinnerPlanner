﻿using DA.DinnerPlanner.Common;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="27.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class DinnerList : ComponentBase
	{
		public ICollection<Model.Dinner> Dinners { get; private set; } = [];
		//[BindProperty]
		public Model.Dinner NewDinner { get; set; } = new();

		protected override async Task OnInitializedAsync()
		{
			Dinners = await Application.Instance.GetAllDinnersAsync(context);
			await base.OnInitializedAsync();
		}

		private async Task NewDinnerSubmittedAsync()
		{
			await Application.Instance.CreateDinnerAsync(context, NewDinner);
			Dinners = await Application.Instance.GetAllDinnersAsync(context);
		}
	}
}
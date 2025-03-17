using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;

namespace DA.DinnerPlanner.Blazor.App.Shared
{
	/// <ChangeLog>
	/// <Create Datum="17.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public partial class CommunicationEntry : ComponentBase
	{
		[Parameter]
		public int UserID { get; set; }
		[Parameter]
		public Communication? Communication { get; set; }
		private int CommTypeId
		{
			get => Communication!.CommunicationType.Id;
			set => Communication!.CommunicationType = dpcontext.CommunicationTypes.Single(ct => ct.Id == value && !ct.Deleted);
		}
	}
}
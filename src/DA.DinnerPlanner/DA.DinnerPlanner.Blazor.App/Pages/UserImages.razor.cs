using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DA.DinnerPlanner.Blazor.App.Pages
{
	/// <ChangeLog>
	/// <Create Datum="17.03.2025" Entwickler="DA" />
	/// <Change Datum="27.03.2025" Entwickler="DA">see https://learn.microsoft.com/de-de/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-9.0#scope-a-database-context-to-the-lifetime-of-the-component (Jira-Nr. DPLAN-80)</Change>
	/// </ChangeLog>
	public partial class UserImages : ComponentBase
	{
		#region Parameters
		[Parameter]
		public int UserID { get; set; }
		#endregion

		[BindProperty]
		public required BufferedSingleFileUploadPhysical FileUpload { get; set; }

		private User? EditingUser { get; set; }

		private DinnerPlannerContext? dpcontext;
		/// <summary>
		/// Identifies whether a db-action is currently in progress
		/// </summary>
		private bool Loading { get; set; } = false;
		protected override async Task OnInitializedAsync()
		{
			if (dpcontext == null)
			{
				dpcontext = await contextFactory.CreateDbContextAsync();
				dpcontext.ConnectionString = configuration.GetConnectionString("da_dinnerplanner-db")!;
			}

			if (Loading)
				return;
			try
			{
				Loading = true;
				if (UserID < 0)
					throw new Exception($"Invalid UserID {UserID}");
				EditingUser = await Application.Instance.GetUserByIdAsync(dpcontext, UserID);
			}
			finally
			{
				Loading = false;
			}
			await Task.CompletedTask;	
		}

		private async Task OnImageDeleteAsync(UserImage image)
		{
			if (Loading)
				return;
			try
			{
				Loading = true;
				image.Delete();
				await dpcontext!.SaveAsync();
			}
			finally
			{
				Loading = false;
			}
		}

		private async Task LoadfileAsync(InputFileChangeEventArgs e)
		{
			// **WARNING!**
			// In the following example, the file is saved without
			// scanning the file's contents. In most production
			// scenarios, an anti-virus/anti-malware scanner API
			// is used on the file before making the file available
			// for download or for use by other systems. 
			// For more information, see the topic that accompanies 
			// this sample.
			if (Loading)
				return;
			try
			{
				Loading = true;
				using (MemoryStream memstream = new())
				{
					await e.File.OpenReadStream(configuration.GetValue<long>("UserProfileImage:FileSizeLimit"))
						.CopyToAsync(memstream);
					EditingUser!.UserImages.Add(new() { Image = memstream.ToArray() });
				}
				await dpcontext!.SaveAsync();
			}
			finally
			{
				Loading = false;
			}
		}

		#region Disposing
		// see: https://learn.microsoft.com/de-de/dotnet/fundamentals/code-analysis/quality-rules/ca1816#example-that-satisfies-ca1816

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				dpcontext?.Dispose();
				dpcontext = null;
			}
		}
		#endregion
	}

	/// <ChangeLog>
	/// <Create Datum="11.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class BufferedSingleFileUploadPhysical
	{
		[Display(Name = "Add File")]
		public IFormFile? FormFile { get; set; }
	}

}
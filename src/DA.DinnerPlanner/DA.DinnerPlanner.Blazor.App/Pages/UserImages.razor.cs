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

		protected override async Task OnInitializedAsync()
		{
			if (UserID < 0)
				throw new Exception($"Invalid UserID {UserID}");
			EditingUser = await Application.Instance.GetUserByIdAsync(dpcontext, UserID);
			await Task.CompletedTask;	
		}

		private async Task OnImageDeleteAsync(UserImage image)
		{
			image.Deleted = true;
			await dpcontext.SaveAsync();
		}

		private async Task OnSaveAsync()
		{
			await dpcontext.SaveAsync();
		}

		private async Task LoadileAsync(InputFileChangeEventArgs e)
		{
			// **WARNING!**
			// In the following example, the file is saved without
			// scanning the file's contents. In most production
			// scenarios, an anti-virus/anti-malware scanner API
			// is used on the file before making the file available
			// for download or for use by other systems. 
			// For more information, see the topic that accompanies 
			// this sample.
			
			using (MemoryStream memstream = new())
			{
				await e.File.OpenReadStream(configuration.GetValue<long>("UserProfileImage:FileSizeLimit"))
					.CopyToAsync(memstream);
				EditingUser!.UserImages.Add(new() { Image = memstream.ToArray() });
			}
			await dpcontext.SaveAsync();
		}
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
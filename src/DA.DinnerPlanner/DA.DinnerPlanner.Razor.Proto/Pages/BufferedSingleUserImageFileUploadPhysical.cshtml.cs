using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model;
using DA.DinnerPlanner.Model.Contracts;
using DA.DinnerPlanner.Model.UnitsTypes;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
	/// <ChangeLog>
	/// <Create Datum="11.02.2025" Entwickler="DA" />
	/// <Change Datum="13.02.2025" Entwickler="DA">User loading stuff added (Jira-Nr. DPLAN-29)</Change>
	/// </ChangeLog>
	public class BufferedSingleUserImageFileUploadPhysicalModel
        :BasePageModel
    {
        private readonly long fileSizeLimitBytes;
        private readonly string[] permittedExtensions = { ".jpg", ".png" };
		public BufferedSingleUserImageFileUploadPhysicalModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context)
			: base(config, backgroundJob, context)
		{
            fileSizeLimitBytes = config.GetValue<long>("UserProfileImage:FileSizeLimit");
            //targetFilePath = config.GetValue<string>("UserProfileImage:StoredFilesPath")!;
		}

        [BindProperty]
        public required BufferedSingleFileUploadPhysical FileUpload { get; set; }
        public string? Result { get; private set; }
		
        [BindProperty(SupportsGet = true)]
		public int? UserID { get; set; }
		public User? EditUser { get; private set; }
		public async Task<IActionResult> OnGetAsync()
		{
			if (UserID == null)
				return NotFound();
			await LoadUserDataAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form!";
                return Page();
            }
			if (FileUpload == null || FileUpload.FormFile == null)
			{
				Result = $"internal error. {nameof(FileUpload)} is null.";
				return BadRequest();
			}

            var formFileContent = await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(FileUpload.FormFile,
                ModelState, permittedExtensions, fileSizeLimitBytes);
		
            if (!ModelState.IsValid)
			{
                Result = "";
				return Page();
			}

			await LoadUserDataAsync();
			if (EditUser == null)
				return NotFound();


			// **WARNING!**
			// In the following example, the file is saved without
			// scanning the file's contents. In most production
			// scenarios, an anti-virus/anti-malware scanner API
			// is used on the file before making the file available
			// for download or for use by other systems. 
			// For more information, see the topic that accompanies 
			// this sample.
			//using var fileStream = System.IO.File.Create(filePath);
			//await fileStream.WriteAsync(formFileContent);

			// To work directly with a FormFile, use the following
			// instead:
			//await FileUpload.FormFile.CopyToAsync(fileStream);
			
			EditUser.UserImages.Add(new UserImage() { Image = formFileContent });
			await db.SaveAsync();
			return RedirectToPage("Index");
		}

		public async Task<IActionResult> OnPostDeleteAsync(int userId, int imageId)
		{
			if (!ModelState.IsValid)
				return Page();
			await LoadUserDataAsync();
			if (EditUser == null)
				return NotFound();
			EditUser.UserImages.First(img => img.Id == imageId).Delete();
			await db.SaveAsync();
			return Page();
		}

		private async Task LoadUserDataAsync()
		{
			EditUser = (await application.GetAllUsersAsync(db)).First(u => u.Id == UserID);
			if (EditUser == null)
				throw new NullReferenceException(nameof(EditUser));
			if (string.IsNullOrEmpty(EditUser.DisplayName))
				EditUser.DisplayName = EditUser.GetDefaultDisplayName();
		}
	}

	/// <ChangeLog>
	/// <Create Datum="11.02.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class BufferedSingleFileUploadPhysical
    {
        //[Required]
        [Display(Name = "Add File")]
        public IFormFile? FormFile { get; set; }
    }
}
using DA.DinnerPlanner.Common;
using DA.DinnerPlanner.Model.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
    /// <ChangeLog>
    /// <Create Datum="11.02.2025" Entwickler="DA" />
    /// </ChangeLog>
    public class BufferedSingleUserImageFileUploadPhysicalModel
        :BasePageModel
    {
        private readonly long fileSizeLimitBytes;
        private readonly string[] permittedExtensions = { "*.jpg", "*.png" };
        private readonly string targetFilePath;
		public BufferedSingleUserImageFileUploadPhysicalModel(IConfiguration config, IBackgroundJobClient backgroundJob, IDinnerPlannerContext context)
			: base(config, backgroundJob, context)
		{
            fileSizeLimitBytes = config.GetValue<long>("UserProfileImage:FileSizeLimit");
            targetFilePath = config.GetValue<string>("UserProfileImage:StoredFilesPath")!;
		}

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }
        public string Result { get; private set; }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form!";
                return Page();
            }
            
            var formFileContent = await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(FileUpload.FormFile,
                ModelState, permittedExtensions, fileSizeLimitBytes);
		
            if (!ModelState.IsValid)
			{
				Result = "Please correct the form!";
				return Page();
			}

            string trustedFileNameForStorage = Path.GetRandomFileName();
            string filePath = Path.Combine(targetFilePath, trustedFileNameForStorage);

			// **WARNING!**
			// In the following example, the file is saved without
			// scanning the file's contents. In most production
			// scenarios, an anti-virus/anti-malware scanner API
			// is used on the file before making the file available
			// for download or for use by other systems. 
			// For more information, see the topic that accompanies 
			// this sample.
			using var fileStream = System.IO.File.Create(filePath);
			await fileStream.WriteAsync(formFileContent);

			// To work directly with a FormFile, use the following
			// instead:
			//await FileUpload.FormFile.CopyToAsync(fileStream);
            return RedirectToPage("Index");
		}
	}

    /// <ChangeLog>
    /// <Create Datum="11.02.2025" Entwickler="DA" />
    /// </ChangeLog>
    public class BufferedSingleFileUploadPhysical
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string? Note { get; set; }
    }
}
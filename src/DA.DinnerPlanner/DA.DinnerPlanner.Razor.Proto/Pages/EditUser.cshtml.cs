using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
    public class EditUserModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int UserID { get; set; }
        public void OnGet()
        {
        }
    }
}

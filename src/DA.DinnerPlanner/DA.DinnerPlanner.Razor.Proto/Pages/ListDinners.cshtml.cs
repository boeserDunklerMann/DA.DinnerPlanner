using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
    public class ListDinnersModel(IConfiguration config) : BasePageModel(config)
    {
        public void OnGet()
        {
        }
    }
}

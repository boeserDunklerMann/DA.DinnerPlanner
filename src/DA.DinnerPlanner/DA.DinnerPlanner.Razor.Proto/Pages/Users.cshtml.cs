using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
    /// <ChangeLog>
    /// <Create Datum="19.12.2024" Entwickler="DA" />
    /// </ChangeLog>
    public class UsersModel : BasePageModel
    {
        public ICollection<Model.User> Users { get; private set; }

        public UsersModel(IConfiguration config) : base(config)
        {
            // DONE DA: das hier zentral auslagern
            Users = application.GetAllUsersAsync().Result;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DA.DinnerPlanner.Razor.Proto.Pages
{
    /// <ChangeLog>
    /// <Create Datum="19.12.2024" Entwickler="DA" />
    /// </ChangeLog>
    public class UsersModel : BasePageModel
    {
        public IReadOnlyCollection<Model.User> Users { get; private set; }

        public UsersModel(IConfiguration config) : base(config)
        {
            Users = context.Users.Where(u=>!u.Deleted).ToList().AsReadOnly();
        }
    }
}
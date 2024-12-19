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
        public IReadOnlyCollection<Model.User> Users { get; private set; }

        public UsersModel(IConfiguration config) : base(config)
        {
            // TODO DA: das hier zentral auslagern
            Users = context.Users
					.Include(nameof(Model.User.AddressList))
					.Include(nameof(Model.User.Allergies))
					.Include(nameof(Model.User.CommunicationList))
					.Include(nameof(Model.User.DinnerAsCook))
					.Include(nameof(Model.User.DinnerAsGuest))
					.Include(nameof(Model.User.DinnerAsHost))
					.Include(nameof(Model.User.Reviews))
					.Include(nameof(Model.User.UserImages))
                    .Include(nameof(Model.User.Languages))
                    .Where(u=>!u.Deleted).ToList().AsReadOnly();
        }
    }
}
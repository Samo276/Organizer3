using Organizer3.Areas.Identity.Data;

namespace Organizer3.Models
{
    public class FunctionsListModel
    {      
            public UserAccess ? Access { get; set; }

            public FunctionsListModel(UserAccess access)
            {
                Access = access;
            }

            public FunctionsListModel()
            {
            }
        }
}

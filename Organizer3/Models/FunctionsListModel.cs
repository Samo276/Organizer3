using Organizer3.Areas.Identity.Data;

namespace Organizer3.Models
{
    public class FunctionsListModel
    {      
        public UserAccess ? Access { get; set; }
        public List<AnnouncerModel> ? NewsList { get; set; }

        public FunctionsListModel(UserAccess? access, List<AnnouncerModel> newsList)
        {
            Access = access;
            NewsList = newsList;
        }

        public FunctionsListModel()
        {
        }
    }
}

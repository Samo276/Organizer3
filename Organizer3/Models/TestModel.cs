using Organizer3.Areas.Identity.Data;

namespace Organizer3.Models
{
    public class TestModel
    {
        public UserAccess Access { get; set; }
        public string userId { get; set; }

        public TestModel(UserAccess access, string userId)
        {
            Access = access;
            this.userId = userId;
        }

        public TestModel()
        {
        }
    }
}

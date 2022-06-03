using Organizer3.Models.EmployeeEditor;
using Organizer3.Models.EmployeeProfile;

namespace Organizer3.Models.MyShop
{
    public class MyShopEmployeeProfileModel
    {
        public UserDataEditModel UserData { get; set; }
        public EmploymentStatusModel EmploymentData { get; set; }
        public string PhotoLocation { get; set; }
        public List<LeaveDisplayModel> LeaveHistory { get; set; }
        public LeaveCounter LeaveUsed { get; set; }
    }
}

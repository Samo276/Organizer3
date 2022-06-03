using Organizer3.Models.EmployeeEditor;
using Organizer3.Models.EmployeeProfile;

namespace Organizer3.Models
{
    public class EmployeeProfileDataModel
    {
        public UserDataEditModel UserData { get; set; }
        public EmploymentStatusModel EmploymentData { get; set; }
        public List<String> ? AccessPrivileges { get; set; }
        public string PhotoLocation { get; set; }
        public string employmentFacility { get; set; }
        public List<LeaveDisplayModel> LeaveHistory { get; set; }
        public LeaveCounter LeaveUsed { get; set; }
        public bool AllowToEdit { get; set; }

        public EmployeeProfileDataModel(UserDataEditModel userData, EmploymentStatusModel employmentData, List<string>? accessPrivileges, string photoLocation, string employmentFacility, List<LeaveDisplayModel> leaveHistory, LeaveCounter leaveUsed, bool underprivelagedAccess)
        {
            UserData = userData;
            EmploymentData = employmentData;
            AccessPrivileges = accessPrivileges;
            PhotoLocation = photoLocation;
            this.employmentFacility = employmentFacility;
            LeaveHistory = leaveHistory;
            LeaveUsed = leaveUsed;
            AllowToEdit = underprivelagedAccess;
        }
    }

}

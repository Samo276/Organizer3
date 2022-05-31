using Organizer3.Models.EmployeeEditor;

namespace Organizer3.Models
{
    public class EmployeeProfileDataModel
    {
        public UserDataEditModel UserData { get; set; }
        public EmploymentStatusModel EmploymentData { get; set; }
        public List<String> ? AccessPrivileges { get; set; }
        public string PhotoLocation { get; set; }
        public string employmentFacility { get; set; }

        public EmployeeProfileDataModel(UserDataEditModel userData, EmploymentStatusModel employmentData, List<string>? accessPrivileges, string photoLocation, string employmentFacility)
        {
            UserData = userData;
            EmploymentData = employmentData;
            AccessPrivileges = accessPrivileges;
            PhotoLocation = photoLocation;
            this.employmentFacility = employmentFacility;
        }
    }

}

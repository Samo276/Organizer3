namespace Organizer3.Models.FacilitiesEditor
{
    public class FacilityProfileModel
    {
        public FacilitiesListModel FacilityData { get; set; }
        public List<UserContactInfo> ShopEmployeeList { get;set; }
        public bool HasEmployeeEditorAccess { get; set; }
    }
}

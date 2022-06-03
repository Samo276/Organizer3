using Organizer3.Areas.Identity.Data;
using Organizer3.Models.FacilitiesEditor;

namespace Organizer3.Models.MyShop
{
    public class MyShopProfileModel
    {
        public FacilitiesListModel FacilityData { get; set; }
        public List<MyShopEmployeeModel> MyShopEmployeeModel { get; set; }
        public List<ShiftInfo> MyShopShiftInfo { get; set; }
        public List<Atendance> MyShopSchedule { get; set; }
    }
}

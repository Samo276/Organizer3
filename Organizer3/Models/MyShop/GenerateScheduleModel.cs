using Organizer3.Areas.Identity.Data;
using Organizer3.Views.MyShop;

namespace Organizer3.Models.MyShop
{
    public class GenerateScheduleModel
    {
        public int Id { get; set; }
        public DateTime FromDay { get; set; }
        public DateTime TillDay { get; set; }
        public int ShiftToEdit { get; set; }
        public List<ShiftInfo> AvailableShifts { get; set; }
        public List<EmployeesInShiftModel> ShiftWithAsignedEmployees { get; set; }
    }
}

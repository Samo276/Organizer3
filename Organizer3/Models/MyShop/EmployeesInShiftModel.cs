using Organizer3.Views.MyShop;

namespace Organizer3.Models.MyShop
{
    public class EmployeesInShiftModel
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public List<EmployeeNameAndIdModel> EmployeesInShift { get;set; } =new List<EmployeeNameAndIdModel>();
    }
}

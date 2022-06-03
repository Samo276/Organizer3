using Organizer3.Views.MyShop;

namespace Organizer3.Models.MyShop
{
    public class EmployeesInShiftModel
    {
        public int Id { get; set; }
        public List<EmployeeNameAndIdModel> Shift { get;set; } =new List<EmployeeNameAndIdModel>();
    }
}

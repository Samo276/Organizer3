namespace Organizer3.Models.EmployeeProfile
{
    public class LeaveCounter
    {
        public int Wypoczynkowy { get; set; }
        public int Okolicznosciowy { get; set; }
        public int Opieka { get; set; }
        public int Szkoleniowy { get; set; }
        public int Zadanie { get; set; }
        public int Bezplatny { get; set; }

        public LeaveCounter()
        {
            Wypoczynkowy = 0;
            Okolicznosciowy = 0;
            Opieka = 0;
            Szkoleniowy = 0;
            Zadanie = 0;
            Bezplatny = 0;
        }
    }
}

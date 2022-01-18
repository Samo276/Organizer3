namespace Organizer3.Models
{
    public class FacilityIdAndNameStorage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SuprevisorId { get; set; }
        public string SuprevisorName { get; set; }

        public FacilityIdAndNameStorage(int id, string name, string suprevisorId, string suprevisorName)
        {
            Id = id;
            Name = name;
            SuprevisorId = suprevisorId;
            SuprevisorName = suprevisorName;
        }
    }
}

namespace Organizer3.Areas.Identity.Data
{
    public class AnnouncerModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public String Sender { get; set; }
        public string MessageContent { get; set; }
        public string ? EditedBy { get; set; }

        public AnnouncerModel(int id, string title, DateTime creationTime, string sender, string messageContent, string? editedBy)
        {
            Id = id;
            Title = title;
            CreationTime = creationTime;
            Sender = sender;
            MessageContent = messageContent;
            EditedBy = editedBy;
        }
    }
}

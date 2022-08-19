namespace Messenger.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public string? Password { get; set; }
        public Group? Group { get; set; }
        public int? GroupId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Messenger.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}

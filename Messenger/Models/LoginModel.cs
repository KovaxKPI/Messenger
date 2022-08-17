using System.ComponentModel.DataAnnotations;

namespace Messenger.Models
{
    public class LoginModel
    {
        [Required]
        public string? Nickname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }   
    }
}

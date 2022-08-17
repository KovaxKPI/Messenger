using System.ComponentModel.DataAnnotations;

namespace Messenger.Models
{
    public class RegisterModel
    {
        [Required]
        public string? Nickname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wrong password")]
        public string? ConfirmPassword { get; set; }
    }
}

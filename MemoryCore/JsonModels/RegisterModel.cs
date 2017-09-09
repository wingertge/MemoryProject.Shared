using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MemoryCore.JsonModels
{
    public class RegisterModel : ValidatableModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MinLength(3), MaxLength(15), RegularExpression("^[a-zA-Z][a-zA-Z0-9_-]+$")]
        public string Username { get; set; }
        [Required, DataType(DataType.Password), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MemoryCore.JsonModels
{
    public class RegisterModel : ValidatableModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required, MinLength(3), MaxLength(15), RegularExpression("^[a-zA-Z][a-zA-Z0-9_-]+$")]
        public virtual string Username { get; set; }
        [Required, DataType(DataType.Password), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public virtual string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        public virtual string PasswordConfirm { get; set; }
    }
}
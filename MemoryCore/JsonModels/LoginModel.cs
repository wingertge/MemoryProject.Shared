using System.ComponentModel.DataAnnotations;

namespace MemoryCore.JsonModels
{
    public class LoginModel : ValidatableModel
    {
        [Required]
        public virtual string Identifier { get; set; }
        [Required]
        public virtual string Password { get; set; }
    }
}
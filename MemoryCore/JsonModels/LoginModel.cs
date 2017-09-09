using System.ComponentModel.DataAnnotations;

namespace MemoryCore.JsonModels
{
    public class LoginModel : ValidatableModel
    {
        [Required]
        public string Identifier { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
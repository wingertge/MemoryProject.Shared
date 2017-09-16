using System.ComponentModel.DataAnnotations;

namespace MemoryCore.JsonModels
{
    /// <summary>
    /// Model for sending data to login function.
    /// </summary>
    /// <seealso cref="MemoryCore.JsonModels.ValidatableModel" />
    public class LoginModel : ValidatableModel
    {
        /// <summary>
        /// The email/username to be used on login
        /// </summary>
        /// <value>The email/username.</value>
        [Required]
        public virtual string Identifier { get; set; }

        /// <summary>
        /// The password to be used on login
        /// </summary>
        /// <value>The password.</value>
        [Required]
        public virtual string Password { get; set; }
    }
}
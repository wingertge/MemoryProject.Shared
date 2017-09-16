using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MemoryCore.JsonModels
{
    /// <summary>
    /// Data model to be sent to the register function.
    /// </summary>
    /// <seealso cref="MemoryCore.JsonModels.ValidatableModel" />
    public class RegisterModel : ValidatableModel
    {
        /// <summary>
        /// The email used for registration.
        /// </summary>
        /// <value>The email.</value>
        [Required, DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        /// <summary>
        /// The username used for registration.
        /// </summary>
        /// <value>The username.</value>
        [Required, MinLength(3), MaxLength(15), RegularExpression("^[a-zA-Z][a-zA-Z0-9_-]+$")]
        public virtual string Username { get; set; }

        /// <summary>
        /// The password used for registration.
        /// </summary>
        /// <value>The password.</value>
        [Required, DataType(DataType.Password), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public virtual string Password { get; set; }

        /// <summary>
        /// Just in case validation for the password. If validated client side, send password again for this.
        /// </summary>
        /// <value>The password confirmation.</value>
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public virtual string PasswordConfirm { get; set; }
    }
}
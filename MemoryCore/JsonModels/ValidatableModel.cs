using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MemoryCore.JsonModels
{
    /// <summary>
    /// Base class for validated JSON models.
    /// </summary>
    public class ValidatableModel
    {
        /// <summary>
        /// Returns true if annotation conditions are valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid()
        {
            var validationContext = new ValidationContext(this);
            return Validator.TryValidateObject(this, validationContext, null, true);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>ErrorCollection.</returns>
        public ErrorCollection Validate()
        {
            var validationContext = new ValidationContext(this);
            try
            {
                Validator.ValidateObject(this, validationContext);
                return new ErrorCollection();
            }
            catch (ValidationException e)
            {
                return e.ValidationResult.MemberNames.ToDictionary(a => a, a => e.ValidationResult.ErrorMessage) as ErrorCollection;
            }
        }
    }
}
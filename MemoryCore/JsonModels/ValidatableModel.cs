using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MemoryCore.JsonModels
{
    public class ValidatableModel
    {
        public bool IsValid()
        {
            var validationContext = new ValidationContext(this);
            return Validator.TryValidateObject(this, validationContext, null, true);
        }

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
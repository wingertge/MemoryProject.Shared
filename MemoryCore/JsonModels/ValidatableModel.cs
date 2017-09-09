using System.ComponentModel.DataAnnotations;

namespace MemoryCore.JsonModels
{
    public class ValidatableModel
    {
        public bool IsValid()
        {
            var validationContext = new ValidationContext(this, null, null);
            return Validator.TryValidateObject(this, validationContext, null, true);
        }
    }
}
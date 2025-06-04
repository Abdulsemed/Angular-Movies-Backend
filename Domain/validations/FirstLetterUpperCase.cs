using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.validations
{
    public class FirstLetterUpperCase : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var firstLetter = value.ToString()[0].ToString();
                if(firstLetter.ToUpper() != firstLetter)
                {
                    return new ValidationResult("First letter should be capital");
                };
            };

            return ValidationResult.Success;
        }
    }
}

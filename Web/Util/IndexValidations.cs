using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace iread_school_ms.Web.Util
{

    public sealed class BiggerThanAttribute : ValidationAttribute
    {
        public string ComparedProperty { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            // Get value of the startDate property
            var property = validationContext.ObjectType.GetProperty(ComparedProperty);
            var valueOfProperty = property.GetValue(validationContext.ObjectInstance, null);
            int indexOfFirstWord = int.Parse(valueOfProperty.ToString());

            int indexOfEndWord = (int)value;
            if (indexOfFirstWord > indexOfEndWord)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}

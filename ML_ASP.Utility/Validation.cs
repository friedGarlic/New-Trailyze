using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ML_ASP.Utility
{
    public class Validation : ValidationAttribute
    {
        private readonly string _dependentProperty;

        public Validation(string dependentProperty)
        {
            _dependentProperty = dependentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var dependentValue = (bool)type.GetProperty(_dependentProperty).GetValue(instance, null);

            if (!dependentValue && value == null)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
            }

            return ValidationResult.Success;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Lookif.Library.Common.Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute//[AllowedExtensions(new string[] { ".jpg", ".png" })]
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                List<string> extensionslst = new List<string>();
                foreach (var item in _extensions)
                {
                    extensionslst.Add(item);
                }

                if (extension != null && !extensionslst.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(extension));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string msg)
        {
            throw new Exception($"پسوند {msg} غیر قابل قبول است");
        }
    }
}

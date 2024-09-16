using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Lookif.Library.Common.Validation;

public class AllowedExtensionsAttribute : ValidationAttribute//[AllowedExtensions(new string[] { ".jpg", ".png" })]
{
    private readonly string[] _extensions;

    public bool NeedException { get; }

    public AllowedExtensionsAttribute(string[] extensions, bool needException = false)
    {
        _extensions = extensions;
        NeedException = needException;
    }

    protected override ValidationResult IsValid(
    object value, ValidationContext validationContext)
    {

        var error = FormatErrorMessage(validationContext.DisplayName);
        var required = CheckIfRequired(validationContext);

        if (value == null && !required)
            return ValidationResult.Success;

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
                return ReturnError(error);
            }
            //nothig wrong so we are good to go
            return ValidationResult.Success;
        }
        //it wasnt placed on IFormFile
        return ReturnError(error);

    }
    private bool CheckIfRequired(ValidationContext validationContext)
    {
        var property = validationContext.ObjectInstance.GetType().GetProperty(validationContext.MemberName);
        if (property.IsDefined(typeof(RequiredAttribute), false))
            return true;
        else
            return false;

    }
    private ValidationResult ReturnError(string error)
    {
        if (!NeedException)
            return new ValidationResult(error);

        throw new Exception(error);
    }
}

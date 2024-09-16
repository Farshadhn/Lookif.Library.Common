using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lookif.Library.Common.Validation;

public class MaxFileSizeAttribute : ValidationAttribute //[MaxFileSize(5* 1024 * 1024)]
{
    private readonly int _maxFileSize;

    public bool NeedException { get; }

    public MaxFileSizeAttribute(int maxFileSize, bool needException = false)
    {
        _maxFileSize = maxFileSize;
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
            if (file.Length > _maxFileSize)
                return ReturnError(error);
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

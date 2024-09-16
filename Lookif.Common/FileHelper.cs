using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lookif.Library.Common;

public static class FileHelper
{
    public async static Task<string> SaveTo(this IFormFile formFile, CancellationToken cancelationToken, string Add = "")
    {
        try
        {
            
            var uploads = (Add == "") ? Directory.GetCurrentDirectory() + "\\uploads" : Add;
            var fileName = $"{Guid.NewGuid()}_{formFile.FileName}";
            if (formFile.Length > 0)
            {
                var filePath = Path.Combine(uploads, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream, cancelationToken);
                }
                return $"{uploads.Replace(Directory.GetCurrentDirectory(),"")}\\{fileName}";
            }
            else
            { throw new FileLoadException("No file is selected"); }
        }
        catch (Exception)
        {

            throw;
        }


    }


}

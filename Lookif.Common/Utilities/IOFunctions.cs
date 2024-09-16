using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lookif.Library.Common.Utilities;

public static class IOFunctions
{
    /// <summary>
    /// Save bytes to directory
    /// </summary>
    /// <param name="fileData"></param>
    /// <param name="FileName"></param>
    /// <param name="Location"></param>
    /// <param name="asbAddress"></param>
    /// <returns></returns>
    public static async Task<string> SaveTo(byte[] fileData, string FileName, string Location = "temp",string absoluteAddress = "")
    {
        string filePath =(string.IsNullOrEmpty(absoluteAddress)?Environment.CurrentDirectory: absoluteAddress) + $"/{Location}/";
        System.IO.FileInfo file = new System.IO.FileInfo(filePath);
        file.Directory.Create(); // If the directory already exists, this method does nothing.
        var prefix = DateTime.Now.ToString("yyyy-MM-dd") + Guid.NewGuid().ToString().Substring(1,5);
        await File.WriteAllBytesAsync($"{filePath}{prefix}-{FileName}", fileData);
        return $"{prefix}-{FileName}";
    }
    public static async Task<string> SaveTo(string fileData, string FileName, string Location = "temp", string absoluteAddress = "")
    {
        var fileInBytearr = Encoding.ASCII.GetBytes(fileData);
        string filePath = (string.IsNullOrEmpty(absoluteAddress) ? Environment.CurrentDirectory : absoluteAddress) + $"/{Location}/";
         
        System.IO.FileInfo file = new System.IO.FileInfo(filePath);
        file.Directory.Create(); // If the directory already exists, this method does nothing.
        var prefix = DateTime.Now.ToString("yyyy-MM-dd") + Guid.NewGuid().ToString().Substring(1, 5);
        await File.WriteAllBytesAsync($"{filePath}{prefix}-{FileName}", fileInBytearr);
        return $"{prefix}-{FileName}";
    }
}

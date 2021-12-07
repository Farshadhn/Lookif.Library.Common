using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookif.Library.Common.Utilities
{
    public static class IOFunctions
    {
        public static async Task SaveTo(byte[] fileData, string FileName, string Location = "temp")
        {
            string filePath = Environment.CurrentDirectory + $"/{Location}/";
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            await File.WriteAllBytesAsync($"{filePath}{DateTime.Now.ToString("yyyy-MM-dd")}-{FileName}", fileData);

        }
        public static async Task SaveTo(string fileData, string FileName, string Location = "temp")
        {
            var fileInBytearr = Encoding.ASCII.GetBytes(fileData);
            string filePath = Environment.CurrentDirectory + $"/{Location}/";
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            await File.WriteAllBytesAsync($"{filePath}{DateTime.Now.ToString("yyyy-MM-dd")}-{FileName}", fileInBytearr);

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESS.Lookif.Library.Common.Utilities
{
    public static class FileExtension
    {
        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public static  List<string> ListAllFiles(string targetDirectory)
        {
            List<string> Files = new List<string>();
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                Files.Add(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                Files.AddRange(ListAllFiles(subdirectory));
            return Files;
        }
    }
}

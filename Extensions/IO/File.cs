using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using Extensions.Models;

namespace Extensions.IO
{
    public class File
    {
        public static string GetFileName(string path)
        {
            char splitter = '/';
            if (path.Contains(@"\")) splitter = '\\';
            string[] parts = path.Split(splitter);
            return parts.LastOrDefault("");
        }

        public static string GetExtension(string path)
        {
            return path.Split('.').LastOrDefault("file");
        }

        public static string _preventNameClash(string fullPath)
        {
            fullPath = Site.MapPath(fullPath);
            if (System.IO.File.Exists(fullPath)) {
                string folderPart = Extensions.IO.Directory.GetFolderPart(fullPath);
                string filename = Extensions.IO.File.GetFileName(fullPath);
                string newFileName = filename.Split('.').First(filename.Split('.').Count() - 1).Join(".") +
                    "-" + Convert.ToInt32(System.IO.Directory.GetFiles(folderPart).Count() + 1) + "." +
                    GetExtension(filename);
                return folderPart.Trim('/') + newFileName;
            }
            return fullPath;
        }
    }
}

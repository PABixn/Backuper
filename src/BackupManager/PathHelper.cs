using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BackupManager
{
    public class PathHelper
    {
        public static string GetDestFileName(string planName, string filePath, string destinationFolder, DateTime dt)
        {
            string extension = Path.GetExtension(filePath);
            filePath = RemoveExtension(filePath.Substring(Path.GetPathRoot(filePath).Length));

            return destinationFolder + filePath + " (" + planName.Replace('/', '_').Replace('\\', '_') + "_" +
                dt.ToString().Replace(' ', '_').Replace('.', '_').Replace(':', '_').Replace('/','_').Replace('\\','_') + ")" + extension;
        }

        private static string RemoveExtension(string filename)
        {
            return filename.Substring(0, filename.Length - Path.GetExtension(filename).Length);
        }
    }
}
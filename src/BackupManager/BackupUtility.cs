using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DBManager;

namespace BackupManager
{
    public class BackupUtility
    {
        private DateTime dt;
        private string planName;

        public BackupUtility(string _planName, DateTime _dt)
        {
            planName = _planName;
            dt = _dt;
        }

        public void MakeBackup(string sourceFolder, string destinationFolder, bool saveFile)
        {
            DateTime fileDT;

            string[] fileList = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);

            foreach(string filePath in fileList)
            {
                fileDT = File.GetLastWriteTimeUtc(filePath);

                if (fileDT.ToString() != DB.GetModifyDate(planName, filePath).ToString())
                {
                    Copy(filePath, destinationFolder);
                    if(saveFile == true) DB.SaveFile(filePath, planName, fileDT);
                }
            }
        }

        private void Copy(string sourceFilePath, string destinationFolder)
        {
            string destFilePath = PathHelper.GetDestFileName(planName, sourceFilePath, destinationFolder, dt);
            Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
            File.Copy(sourceFilePath, destFilePath);
        }
    }
}

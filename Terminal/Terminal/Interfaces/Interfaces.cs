using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Terminal.Interfaces
{
    public interface IFileService
    {
        bool FileExists(string filePath);
    }

    public interface IPclPrintService
    {
        void Print(string IpAddress,int Port, byte[] content);
    }

    public class FileService : IFileService
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string checkImageExist(string imageName)
        {
            if (File.Exists(imageName + ".xml"))
            {
                return imageName + ".xml";
            }
            else if (File.Exists(imageName + ".png"))
            {
                return imageName + ".png";
            }
            else if (File.Exists(imageName + ".pdf"))
            {
                return imageName + ".pdf";
            }
            else { return imageName ; }
        }
    }
}

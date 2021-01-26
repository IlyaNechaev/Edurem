using Edurem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface IFileService
    {
        public void UploadFile(FileModel file);

        public void DownloadFile(FileModel file);

        public string GetFileText(FileModel file);

        public string GetFileText(string filePath);
    }
}

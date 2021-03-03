using Edurem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface IFileService
    {
        public Task<FileModel> UploadFile(Stream stream, string filePath, string fileName);

        public Task<FileStream> GetFile(int fileId);

        public string GetFileText(FileModel file);

        public string GetFileText(string filePath);
    }
}

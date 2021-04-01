using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface IFileService
    {
        // Загрузка файлов на сервер
        public Task<FileModel> UploadFile(Stream stream, string filePath, string fileName);

        // Получить модель файла из БД
        public Task<FileModel> GetFile(int fileId);

        // Получить FileStream файла
        public FileStream OpenFile(FileModel fileModel);

        // Получить FileStream файла
        public FileStream OpenFile(int fileId);

        // Удалить файл из файловой системы и БД
        public Task RemoveFile(int fileId, bool forceRemove = false);

        public Task<bool> HasReferences(int fileId);

        public string GetFileText(FileModel file);

        public string GetFileText(string filePath);

        public string GetExtensionClass(string extension) => extension switch
        {
            "doc" or "docx" or "odt" => "far fa-file-word",
            "xls" or "xlsx" => "far fa-file-excel",
            "pdf" => "far fa-file-pdf",
            "txt" => "far fa-file-alt",
            "py" or "pyw" or "pyd" or "cs" or "cpp" or "php" or "html" or "css" => "far fa-file-code",
            "png" or "jpg" or "jpeg" or "gif" => "far fa-file-image",
            _ => "far fa-file"
        };
    }
}

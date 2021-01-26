using Edurem.Extensions;
using Edurem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class FileService : IFileService
    {
        IWebHostEnvironment AppEnvironment { get; init; }
        IConfiguration Configuration { get; set; }

        public FileService(IWebHostEnvironment appEnvironment,
                           [FromServices] IConfiguration configuration)
        {
            AppEnvironment = appEnvironment;
            Configuration = configuration;
        }

        public void UploadFile(FileModel file)
        {

        }

        public void DownloadFile(FileModel file)
        {

        }

        public string GetFileText(FileModel file)
        {
            var text = string.Empty;

            using (var fileStream = new FileStream(AppEnvironment.WebRootPath + file.GetFullPath(), FileMode.Open))
            {
                // Преобразование строки в байты
                var array = new byte[fileStream.Length];

                // Считывание данных
                fileStream.Read(array, 0, array.Length);

                // Декодирование байтов в строку
                text = System.Text.Encoding.Default.GetString(array);
            }

            return text;
        }

        public string GetFileText(string filePath)
        {
            var text = string.Empty;

            using (var fileStream = new FileStream(AppEnvironment.WebRootPath + filePath, FileMode.Open))
            {
                // Преобразование строки в байты
                var array = new byte[fileStream.Length];

                // Считывание данных
                fileStream.Read(array, 0, array.Length);

                // Декодирование байтов в строку
                text = System.Text.Encoding.Default.GetString(array);
            }

            return text;
        }
    }
}

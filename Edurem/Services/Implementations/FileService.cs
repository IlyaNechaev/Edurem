using Edurem.Data;
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
        IRepositoryFactory RepositoryFactory { get; init; }

        public FileService(IWebHostEnvironment appEnvironment,
                           [FromServices] IRepositoryFactory repositoryFactory)
        {
            AppEnvironment = appEnvironment;
            RepositoryFactory = repositoryFactory;
        }

        public async Task<FileModel> UploadFile(Stream stream, string filePath, string fileName)
        {
            var FileRepository = RepositoryFactory.GetRepository<FileModel>();

            string fullFilePath = Path.Combine(AppEnvironment.WebRootPath, filePath, fileName);
            string directoryPath = Path.Combine(AppEnvironment.WebRootPath, filePath);

            // Создаем директорию, если ее нет
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var fileModel = new FileModel() { Name = fileName, Path = filePath };

            // Если файл не существует
            if (!File.Exists(fullFilePath))
            {
                // Сохраняем данные в БД
                await FileRepository.Add(fileModel);
            }

            using (var fileStream = File.Exists(fullFilePath) ? File.OpenWrite(fullFilePath) : File.Create(fullFilePath))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }

            // Возвращаем модель файла
            return fileModel;
        }

        public async Task<FileStream> GetFile(int fileId)
        {
            var FileRepository = RepositoryFactory.GetRepository<FileModel>();

            // Получаем модель файла из БД
            var file = await FileRepository.Get(f => f.Id == fileId);

            // Создаем FileStream для файла
            var fileStream = new FileStream(file.GetFullPath(), FileMode.Open);

            return fileStream;
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

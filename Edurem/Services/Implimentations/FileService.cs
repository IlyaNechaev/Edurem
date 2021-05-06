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

        public async Task<FileModel> UploadFile(Stream stream, string filePath, string fileName, bool dbUpload = true)
        {

            string fullFilePath = Path.Combine(AppEnvironment.WebRootPath, filePath, fileName);
            string directoryPath = Path.Combine(AppEnvironment.WebRootPath, filePath);

            // Создаем директорию, если ее нет
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            
            using (var fileStream = File.Exists(fullFilePath) ? File.OpenWrite(fullFilePath) : File.Create(fullFilePath))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }

            var fileModel = new FileModel()
            {
                Name = fileName,
                Path = filePath, 
                Size = stream.Length
            };

            if (dbUpload)
            {
                var FileRepository = RepositoryFactory.GetRepository<FileModel>();
                // Если файл не существует
                if ((await FileRepository.Get(file => file.Name == fileModel.Name && file.Path == fileModel.Path)) is null)
                {
                    // Сохраняем данные в БД
                    await FileRepository.Add(fileModel);
                }
                else
                {
                    fileModel = await FileRepository.Get(file => file.Name == fileModel.Name && file.Path == fileModel.Path);
                }
            }

            // Возвращаем модель файла
            return fileModel;
        }

        public async Task<FileModel> GetFile(int fileId)
        {
            var FileRepository = RepositoryFactory.GetRepository<FileModel>();
            FileModel file = new();
            // Получаем модель файла из БД
            try
            {
                file = (await FileRepository.Find(f => f.Id == fileId)).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return file;
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

        public FileStream OpenFile(FileModel fileModel)
        {
            // Полный путь до файла
            var filePath = Path.Combine(AppEnvironment.WebRootPath, fileModel.GetFullPath());

            // Создаем FileStream для файла
            var fileStream = new FileStream(filePath, FileMode.Open);

            return fileStream;
        }

        public FileStream OpenFile(int fileId)
        {
            var file = GetFile(fileId).Result;

            var fileStream = OpenFile(file);

            return fileStream;
        }

        public async Task RemoveFile(int fileId, bool forceRemove)
        {
            var FileRepository = RepositoryFactory.GetRepository<FileModel>();

            if (!await HasReferences(fileId) || forceRemove)
            {
                var file = await FileRepository.Get(f => f.Id == fileId);

                // Удалить файл из БД
                await FileRepository.Delete(file);

                // Удалить файл из файловой системы
                var filePath = Path.Combine(AppEnvironment.WebRootPath, file.GetFullPath());
                File.Delete(filePath);
            }
            else
            {
                throw new Exception("Невозможно удалить файл, поскольку на него ссылаются другие сущности");
            }

            return;
        }

        public async Task<bool> HasReferences(int fileId)
        {
            var PostFileRepository = RepositoryFactory.GetRepository<PostFile>();
            if ((await PostFileRepository.Get(postFile => postFile.FileId == fileId)) is not null) return true;

            return false;
        }

        public Stream GetFileStream(FileModel file)
        {
            return File.OpenRead(Path.Combine(AppEnvironment.WebRootPath, file.GetFullPath()));
        }

        public string GetFullPath(params string[] paths)
        {
            var fullPath = AppEnvironment.WebRootPath;
            foreach (var path in paths)
            {
                fullPath = Path.Combine(fullPath, path);
            }
            return fullPath;
        }
    }
}

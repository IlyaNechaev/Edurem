using Edurem.Data;
using Edurem.Extensions;
using Edurem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

            // Если файл не находится там, куда его нужно загрузить
            if (!((stream as FileStream)?.Name.Equals(fullFilePath) ?? false))
            {
                try
                {
                    using (var fileStream = File.Exists(fullFilePath) ? File.OpenWrite(fullFilePath) : File.Create(fullFilePath))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
                file = (await FileRepository.Get(f => f.Id == fileId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return file;
        }

        public async Task<FileModel> GetFile(string path)
        {
            var FileRepository = RepositoryFactory.GetRepository<FileModel>();
            FileModel file = new();
            // Получаем модель файла из БД
            try
            {
                file = (await FileRepository.Get(f => f.Path.Equals(path)));
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

                try
                {
                    // Удалить файл из файловой системы
                    var filePath = Path.Combine(AppEnvironment.WebRootPath, file.GetFullPath());
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    // Если невозможно удалить файл, то метод завершается,
                    // чтобы не удалять информацию о файле из БД
                    return;
                }

                if (forceRemove)
                    // Удаление файла из связуюших таблиц
                    await ForceRemoveFile(fileId);

                // Удалить файл из БД
                await FileRepository.Delete(file);
            }
            else
            {
                throw new Exception("Невозможно удалить файл, поскольку на него ссылаются другие сущности");
            }

            return;
        }

        private async Task ForceRemoveFile(int fileId)
        {
            if (!await HasReferences(fileId)) return;

            var PostFileRepository = RepositoryFactory.GetRepository<PostFile>();

            var postFile = (await PostFileRepository.Get(postFile => postFile.FileId == fileId));

            await PostFileRepository.Delete(postFile);
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

        public Stream GetFileStream(string filePath)
        {
            if (File.Exists(filePath))
                return File.OpenRead(filePath);
            return File.OpenRead(Path.Combine(AppEnvironment.WebRootPath, filePath));
        }

        public Stream ZipFiles(List<ZipItem> zipItems)
        {

            var zipStream = new MemoryStream();

            using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var zipItem in zipItems)
                {
                    var entry = zip.CreateEntry(zipItem.Name);
                    using (var entryStream = entry.Open())
                    {
                        zipItem.Content.CopyTo(entryStream);
                    }
                }
            }
            zipStream.Position = 0;
            return zipStream;
        }
    }
}

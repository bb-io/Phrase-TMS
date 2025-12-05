using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System.Text;

namespace PhraseTMSTests.Base
{
    public class FileManager : IFileManagementClient
    {
        private readonly string inputFolder;
        private readonly string outputFolder;

        public FileManager()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;


            var testFilesPath = Path.Combine(projectDirectory, "TestFiles");
            inputFolder = Path.Combine(testFilesPath, "Input");
            outputFolder = Path.Combine(testFilesPath, "Output");

            Directory.CreateDirectory(inputFolder);
            Directory.CreateDirectory(outputFolder);
        }


        public Task<Stream> DownloadAsync(FileReference reference)
        {
            var path = Path.Combine(inputFolder, reference.Name);
            Assert.IsTrue(File.Exists(path), $"File not found at: {path}");
            var bytes = File.ReadAllBytes(path);

            var stream = new MemoryStream(bytes);
            return Task.FromResult((Stream)stream);
        }

        public string ReadOutputAsString(FileReference reference)
        {
            var path = Path.Combine(outputFolder, reference.Name);
            Assert.IsTrue(File.Exists(path), $"File not found at: {path}");
            return File.ReadAllText(path, Encoding.UTF8)!;
        }

        public Task<FileReference> UploadAsync(Stream stream, string contentType, string fileName)
        {
            var path = Path.Combine(outputFolder, fileName);
            new FileInfo(path).Directory.Create();
            using (var fileStream = File.Create(path))
            {
                stream.CopyTo(fileStream);
            }

            return Task.FromResult(new FileReference() { Name = fileName, ContentType = contentType });
        }

        public async Task<FileReference> UploadTestFileAsync(string fileName, string contentType = "text/plain")
        {
            var testFilePath = Path.Combine(inputFolder, fileName);
            Assert.IsTrue(File.Exists(testFilePath), $"Test file not found at: {testFilePath}");

            using var fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read);

            var fileReference = new FileReference
            {
                Name = fileName,
                ContentType = contentType
            };

            return fileReference;
        }
    }
}

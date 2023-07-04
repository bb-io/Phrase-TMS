namespace Apps.PhraseTMS.Models.Files.Requests
{
    public class UploadFileRequest
    {
        public byte[] File { get; set; }

        public string FileName { get; set; }
    }
}

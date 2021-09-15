using System;

namespace iread_school_ms.Web.Dto.Attachment
{
    public class AttachmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
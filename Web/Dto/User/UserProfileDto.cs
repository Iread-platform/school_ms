using System;
using iread_school_ms.Web.Dto.Attachment;

namespace iread_school_ms.Web.Dto.User
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public int Level { get; set; }
        public DateTime BirthDay { get; set; }
        public AttachmentDTO AvatarAttachment { get; set; }
        public AttachmentDTO CustomPhotoAttachment { get; set; }
    }
}
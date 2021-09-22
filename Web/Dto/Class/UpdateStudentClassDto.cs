using System.ComponentModel.DataAnnotations;

namespace iread_school_ms.Web.Dto.Class
{
    public class UpdateStudentClassDto
    {
        [Required]
        public int ClassMemberId { get; set; }
        [Required]
        public int ClassId { get; set; }
    }
}
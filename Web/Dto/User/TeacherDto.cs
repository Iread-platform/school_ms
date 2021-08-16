using System;
using System.ComponentModel.DataAnnotations;

namespace iread_school_ms.Web.Dto.User
{

    public class TeacherDto
    {

        [Required(AllowEmptyStrings = false)]
        public string MemberId { get; set; }
    }
}
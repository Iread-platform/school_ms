using System;
using System.ComponentModel.DataAnnotations;

namespace iread_school_ms.Web.Dto.User
{

    public class StudentDto
    {

        [Required(AllowEmptyStrings = false)]
        public string MemberId { get; set; }
    }
}
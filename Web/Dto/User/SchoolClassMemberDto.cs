using System.Collections.Generic;
using iread_school_ms.Web.Dto.Class;

namespace iread_school_ms.Web.Dto.User
{
    public class SchoolClassMemberDto
    {
        public string MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<InnerClassDto> Classes { get; set; }

    }
}
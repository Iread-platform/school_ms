using System.Collections.Generic;
using iread_school_ms.Web.Dto.Class;

namespace iread_school_ms.Web.Dto.School
{
    public class SchoolDto
    {
        public int SchoolId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public List<InnerClassDto> Classes { get; set; }
        public bool Archived { get; set; }


    }
}
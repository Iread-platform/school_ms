using System.Collections.Generic;
using iread_school_ms.Web.Dto.Class;

namespace iread_school_ms.Web.Dto.School
{
    public class SchoolAndClassDto
    {
        public int SchoolId { get; set; }
        public string SchoolTitle { get; set; }
        
        public string SchoolMembershipType { get; set; }
        public IEnumerable<InnerClassDto> Classes { get; set; }
        
        // public int ClassId { get; set; }
        //
        // public string ClassTitle { get; set; }
        //
        //
        // public bool Archived { get; set; }
    }
}
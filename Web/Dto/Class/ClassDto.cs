using iread_school_ms.Web.Dto.School;

namespace iread_school_ms.Web.Dto.Class
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string Title { get; set; }
        public SchoolDto School { get; set; }

    }
}
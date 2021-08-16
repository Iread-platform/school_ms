using System.ComponentModel.DataAnnotations;

namespace iread_school_ms.Web.Dto.School
{
    public class SchoolCreateDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Location { get; set; }

    }
}
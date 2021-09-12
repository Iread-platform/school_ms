using System.ComponentModel.DataAnnotations;

namespace iread_school_ms.Web.Dto.User
{
    public class UpdateMemberDto
    {
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
    }
}
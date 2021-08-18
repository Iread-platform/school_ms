using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iread_school_ms.DataAccess.Data.Type;

namespace iread_school_ms.DataAccess.Data.Entity
{
    [Table("SchoolMembers")]
    public class SchoolMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int SchoolMemberId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        public string MemberId { get; set; }

        [Required]
        [EnumDataType(typeof(SchoolMembershipType), ErrorMessage = "{} value not valid, should be one of [Teacher, Student, SchoolManager]")]
        public string SchoolMembershipType { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iread_school_ms.DataAccess.Data.Type;

namespace iread_school_ms.DataAccess.Data.Entity
{
    [Table("ClassMembers")]
    public class ClassMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ClassMemberId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        public int MemberId { get; set; }

        [Required]
        [EnumDataType(typeof(ClassMembershipType), ErrorMessage = "{} value not valid, should be one of [Teacher, Student]")]
        public string ClassMembershipType { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }

    }
}
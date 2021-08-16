using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_school_ms.DataAccess.Data.Entity
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ClassId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public int SchoolId { get; set; }
        public School School { get; set; }
        public List<ClassMember> Members { get; set; }

    }
}
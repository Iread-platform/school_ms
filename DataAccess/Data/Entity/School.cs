using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_school_ms.DataAccess.Data.Entity
{
    [Table("Schools")]
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int SchoolId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Location { get; set; }
        public List<SchoolMember> Members { get; set; }
        public List<Class> Classes { get; set; }


    }
}
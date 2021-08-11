using System;
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

        public string Title { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_school_ms.DataAccess.Data.Entity
{
    [Table("SchoolManagers")]
    public class SchoolManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int SchoolManagerId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {} field is required")]
        public int SchoolId { get; set; }

        public School School { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {} field is required")]
        public int ManagerId { get; set; }


    }
}
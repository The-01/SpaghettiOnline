using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Models
{
    public class Page
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [MinLength(3, ErrorMessage = "Page title should be atleast 3 characters!")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Slug { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        [MinLength(10, ErrorMessage = " Page content should be atleast 10 characters!")]
        public String Content { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}

using Microsoft.AspNetCore.Http;
using SpaghettiOnline.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [MinLength(3, ErrorMessage = "Name should be atleast 3 characters long!")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        [MinLength(10, ErrorMessage = "Give a description of atleast 10 characters!")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string Image { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Slug { get; set; }

        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a category.")]
        public int CategoryId { get; set; }


        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }

        [Display(Name = "Check! if product is popular with users")]
        public bool IsPopular { get; set; }
    }
}

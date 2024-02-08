using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models
{
    public class Product
    {
        [Key]                     // Data annotation which indicates that 'id' will be primary key
        public int Id { get; set; }      //prop key
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public  string? ISBN { get; set; }
        [Required]
        public string? Author { get; set;}

        [Required]
        [Range(1, 1000)]
        [Display(Name = "ListPrice")]          //For product
        public double? ListPrice { get; set; }

        [Required]
        [Range(1,1000)]
        [Display(Name ="Price for 1-50")]            //for quantity
        public double Price { get; set;}

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 50+")]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 100+")]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }            //foreign key to the category table

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }    //this category property is used for foreign key navigation to the categoryid
        [ValidateNever]
        public string? ImageUrl { get; set; }
    }

}

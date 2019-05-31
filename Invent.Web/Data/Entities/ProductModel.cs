using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Data.Entities
{
    public class ProductModel : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Model name")]
        public string Name { get; set; }

        [Display(Name = "Date of creation")]
        public DateTime DateOfCreation { get; set; }

        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

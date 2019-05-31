using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Invent.Web.Data.Entities
{
    public class Manufacturer : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime DateOfCreation { get; set; }

        public ICollection<ProductModel> ProductModelss { get; set; }
    }
}

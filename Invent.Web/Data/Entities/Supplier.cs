using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Data.Entities
{
    public class Supplier : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public string Address { get; set; }

        
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal code")]
        public string PostCode { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Website { get; set; }

        
        public string NIPC { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

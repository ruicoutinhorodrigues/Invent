using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Invent.Web.Data.Entities
{
    public class Inventory : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Display(Name = "User name")]
        public string UserName { get; set; }



        [Display(Name = "Date of creation")]
        [DataType(DataType.Date)]
        public DateTime DateOfCreation { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}

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

        public bool NotAvailable { get; set; }


        [Display(Name = "Date of creation")]
        [DataType(DataType.Date)]
        public DateTime DateOfCreation { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                //Warning: this must be your server path
                return $"https://inventory2019.ddns.net{this.ImageUrl.Substring(1)}";
            }
        }

        public ICollection<Product> Products { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}

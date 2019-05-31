using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Data.Entities
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string GuyName { get; set; }

        [Required]
        [Display(Name = "Contact")]
        [DataType(DataType.EmailAddress)]
        public string GuyContact { get; set; }

        [Required]
        [Display(Name = "Reference code")]
        
        public string ProdReferenceCode { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Report")]
        public string Trouble { get; set; }

        [MaxLength(200)]
        public string Remedy { get; set; }

        [Display(Name = "Closed issue")]
        public bool ClosedIssue { get; set; }

        [Display(Name = "Open date")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }

        [Display(Name = "Closed date")]
        [DataType(DataType.Date)]
        public DateTime? CloseDate { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
    }
}

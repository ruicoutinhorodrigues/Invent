using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invent.Web.Data.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [Display(Name = "Reference")]
        public string ReferenceCode { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? Value { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Entry date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Last change")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime LastChangeDate { get; set; }

        //[Required]
        //public IdentityUser User { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                //ATENÇÃO --- MUDAR PARA NOME DO SERVIDOR
                return $"https://inventory2019.ddns.net{this.ImageUrl.Substring(1)}";
            }
        }


        [Required(ErrorMessage ="Please choose the inventory")]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        [Required(ErrorMessage = "Please choose the location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Required(ErrorMessage = "Please choose the status")]
        public int StatusId { get; set; }
        public Status Status  { get; set; }

        [Required(ErrorMessage = "Please choose the category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Please choose the model")]
        
        public int ProductModelId { get; set; }

        [Display(Name = "Model")]
        public  ProductModel ProductModel { get; set; }

        [Required(ErrorMessage = "Please choose the supplier")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int? SizeId { get; set; }
        public Size Size { get; set; }

        public int? ColorId { get; set; }
        public Color Color { get; set; }
    }
}

using Invent.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Models
{
    public class ChoicesViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Location> Locations { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        public List<Status> Status { get; set; }
        public List<ProductModel> ProductModels { get; set; }

        public List<Supplier> Suppliers { get; set; }
        //public List<Color> Colors { get; set; }
        //public List<Size> Sizes { get; set; }
    }
}

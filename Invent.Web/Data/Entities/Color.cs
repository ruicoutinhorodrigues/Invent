using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Data.Entities
{
    public class Color
    {
        public int Id { get; set; }

        [Display(Name ="Predominant color")]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

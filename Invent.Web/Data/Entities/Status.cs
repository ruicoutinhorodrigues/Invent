﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Data.Entities
{
    public class Status : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}

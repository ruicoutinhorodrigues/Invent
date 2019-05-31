using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Models
{
    public class StatViewModel
    {
        public List<VLocation> VLocations { get; set; }
        public List<VCategory> VCategories { get; set; }
        public List<VSupplier> VSuppliers { get; set; }
        public List<VStatus> VStatus { get; set; }

        public List<ValueMonth> ValueMonth { get; set; }
    }

    public class ValueMonth
    {
        public decimal? TotalValue { get; set; }
        public DateTime Month { get; set; }
    }

    public class VLocation
    {
        public int DimensionId { get; set; }
        public string Dimension { get; set; }
        public int Quantity { get; set; }
    }

    public class VCategory
    {
        public int DimensionId { get; set; }
        public string Dimension { get; set; }
        public int Quantity { get; set; }
    }

    public class VSupplier
    {
        public int DimensionId { get; set; }
        public string Dimension { get; set; }
        public int Quantity { get; set; }
    }

    public class VStatus
    {
        public int DimensionId { get; set; }
        public string Dimension { get; set; }
        public int Quantity { get; set; }
    }
}

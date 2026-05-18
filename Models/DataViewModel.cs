using System.Collections.Generic;

namespace App_practical.Models
{
    public class DataViewModel
    {
        public double Result { get; set; }
        public string? ErrorMessage { get; set; }
        public List<Variant> Variants { get; set; } = new List<Variant>();
    }
}

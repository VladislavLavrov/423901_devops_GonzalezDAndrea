using System.ComponentModel.DataAnnotations;

namespace App_practical.Models
{
    public class Variant
    {
        [Key]
        public int Id { get; set; }
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public string Operation { get; set; } = string.Empty;
        public double Result { get; set; }
    }
}
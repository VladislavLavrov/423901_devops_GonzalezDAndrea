using System.ComponentModel.DataAnnotations;

namespace App_practical.Models // Asegúrate de que sea tu namespace real
{
    public class Variant
    {
        [Key]
        public int Id { get; set; }

        // Propiedad opcional para guardar el nombre del cálculo
        public string? Name { get; set; }

        // Cambiamos Operand1 por Value1 para que coincida con el servicio Kafka
        public double Value1 { get; set; }

        // Cambiamos Operand2 por Value2
        public double Value2 { get; set; }

        public string Operation { get; set; } = string.Empty;

        public double Result { get; set; }
    }
}
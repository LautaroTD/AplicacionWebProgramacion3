using System.ComponentModel.DataAnnotations;

namespace AplicacionWebProgramacion3.Models
{
    public class Suelos
    {
        [Key]
        public int id { get; set; }
        public string Nombre { get; set; }
        public int pH { get; set; }
        public string Tipo { get; set; }
    }
}

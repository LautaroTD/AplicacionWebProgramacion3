namespace AplicacionWebProgramacion3.Models
{
    public partial class Plantas
    {
        public int Id { get; set; }

        public string NombreCientifico { get; set; } = string.Empty;

        public string NombreVulgar { get; set; } = string.Empty;

        public string Autor { get; set; } = string.Empty;

        public string EpocaFloracion { get; set; } = string.Empty;

        public int? AlturaMaxima { get; set; } 

        public string Descripcion { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;
    }
}

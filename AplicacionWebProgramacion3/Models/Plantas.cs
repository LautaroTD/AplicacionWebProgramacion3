namespace AplicacionWebProgramacion3.Models
{
    public partial class Plantas
    {
        public int Id { get; set; }

        public string NombreCientifico { get; set; }

        public string NombreVulgar { get; set; }

        public string Autor { get; set; }

        public string EpocaFloracion { get; set; }

        public int? AlturaMaxima { get; set; }

        public string Descripcion { get; set; }
        public string Imagen { get; set; }
    }
}

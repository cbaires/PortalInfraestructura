namespace PortalInfraestructura.Application.Menu.DTO
{
    public class ModuloMenuDto
    {
        public int? Id { get; set; }
        public int? Nivel { get; set; }
        public string? Nombre { get; set; }
        public string? Icono { get; set; }
        public string? Url { get; set; }
        public int? IdModuloPadre { get; set; }
    }
}

namespace PortalIAAS.Web.UI.Notificaciones
{
    public class Notificacion
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string? Origen { get; set; } = string.Empty;
        public DateTimeOffset Fecha { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public bool Leida { get; set; }
    }
}

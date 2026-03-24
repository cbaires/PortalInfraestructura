using MudBlazor;

namespace PortalInfraestructura.Web.UI.Notificaciones
{
    public static class EstiloNotificacion
    {
        public static string ObtenerIcono(TipoNotificacion tipo)
        {
            return tipo switch
            {
                TipoNotificacion.Informacion => Icons.Material.Filled.Info,
                TipoNotificacion.Exito => Icons.Material.Filled.CheckCircle,
                TipoNotificacion.Advertencia => Icons.Material.Filled.Warning,
                TipoNotificacion.Error => Icons.Material.Filled.Error,
                _ => Icons.Material.Filled.Notifications
            };
        }
        public static Color ObtenerColor(TipoNotificacion tipo)
        {
            return tipo switch
            {
                TipoNotificacion.Informacion => Color.Info,
                TipoNotificacion.Exito => Color.Success,
                TipoNotificacion.Advertencia => Color.Warning,
                TipoNotificacion.Error => Color.Error,
                _ => Color.Default
            };
        }
        public static Severity ObtenerSeveridad(TipoNotificacion tipo)
        {
            return tipo switch
            {
                TipoNotificacion.Informacion => Severity.Info,
                TipoNotificacion.Exito => Severity.Success,
                TipoNotificacion.Advertencia => Severity.Warning,
                TipoNotificacion.Error => Severity.Error,
                _ => Severity.Normal
            };
        }
    }
}
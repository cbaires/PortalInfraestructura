namespace PortalInfraestructura.Web.UI.Notificaciones
{
    public class NotificacionService
    {
        private const int _maxNotificaciones = 100;

        private readonly List<Notificacion> _notificaciones = [];
        public int TodasNotificaciones => _notificaciones.Count;
        public int NotificacionesNoLeidas => _notificaciones.Count(n => !n.Leida);
        public event Action<Notificacion>? OnNotificacionAgregada;
        public event Action<Notificacion>? OnNotificacionEliminada;
        public event Action<Notificacion>? OnNotificacionLeida;
        public event Action? OnTodasNotificacionLeidas;

        public IReadOnlyList<Notificacion> ObtenerNotificaciones(int cantidad = _maxNotificaciones)
        {
            return [.. _notificaciones.OrderByDescending(n => n.Fecha).Take(cantidad)];
        }
        public IReadOnlyList<Notificacion> ObtenerNotificacionesNoLeidas(int cantidad = _maxNotificaciones)
        {
            return [.. _notificaciones.Where(n => !n.Leida).OrderByDescending(n => n.Fecha).Take(cantidad)];
        }
        public void Agregar(Notificacion notificacion)
        {
            if (_notificaciones.Count >= _maxNotificaciones)
            {
                _notificaciones.RemoveAt(0);
            }

            _notificaciones.Add(notificacion);
            OnNotificacionAgregada?.Invoke(notificacion);
        }
        public void Eliminar(Guid id)
        {
            var notificacion = _notificaciones.FirstOrDefault(n => n.Id == id);
            if (notificacion != null)
            {
                _notificaciones.Remove(notificacion);
                OnNotificacionEliminada?.Invoke(notificacion);
            }
        }
        public void MarcarComoLeida(Guid id)
        {
            var notificacion = _notificaciones.FirstOrDefault(n => n.Id == id);
            if (notificacion != null)
            {
                notificacion.Leida = true;
                OnNotificacionLeida?.Invoke(notificacion);
            }
        }
        public void MarcarTodasComoLeidas()
        {
            _notificaciones.ForEach(n =>
            {
                if (!n.Leida)
                {
                    n.Leida = true;
                }
            });
            OnTodasNotificacionLeidas?.Invoke();
        }
    }
}

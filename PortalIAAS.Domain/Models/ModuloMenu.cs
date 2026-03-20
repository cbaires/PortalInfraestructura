using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Domain.Models
{
    public class ModuloMenu
    {
        public int? Id { get; set; }
        public int? Nivel { get; set; }
        public string? Nombre { get; set; }
        public string? Icono { get; set; }
        public string? Url { get; set; }
        public int? IdModuloPadre { get; set; }
    }
}

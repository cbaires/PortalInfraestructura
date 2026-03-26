using System;
using System.Collections.Generic;
using System.Text;

namespace PortalInfraestructura.Application.Common.Exceptions
{
    public class UsuarioNoIdentificadoException : AppException
    {
        public UsuarioNoIdentificadoException(string mensaje) : base(mensaje) { }

        public UsuarioNoIdentificadoException(string mensaje, Exception innerException) : base(mensaje, innerException) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PortalInfraestructura.Application.Common.Exceptions
{
    public class AccesoDatosException : AppException
    {
        public AccesoDatosException(string mensaje) : base(mensaje) { }

        public AccesoDatosException(string mensaje, Exception innerException) : base(mensaje, innerException) { }
    }
}

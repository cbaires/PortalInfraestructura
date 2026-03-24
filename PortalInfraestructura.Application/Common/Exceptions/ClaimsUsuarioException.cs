using System;
using System.Collections.Generic;
using System.Text;

namespace PortalInfraestructura.Application.Common.Exceptions
{
    public class ClaimsUsuarioException(string mensaje) : AppException(mensaje)
    {
    }
}

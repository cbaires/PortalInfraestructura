using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Application.Common.Exceptions
{
    public class ClaimsUsuarioException(string mensaje) : AppException(mensaje)
    {
    }
}

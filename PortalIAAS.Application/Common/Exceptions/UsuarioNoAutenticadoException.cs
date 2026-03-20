using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Application.Common.Exceptions
{
    public class UsuarioNoAutenticadoException(string mensaje) : AppException(mensaje)
    {
    }
}

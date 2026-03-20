using System;
using System.Collections.Generic;
using System.Text;

namespace PortalIAAS.Application.Common.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string mensaje) : base(mensaje) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTwoFactorAuthentication.Models
{
    public class DataValidateGoogle
    {
        public string Cuenta { get; set; }
        public string Emisor { get; set; }
        public string Codigo { get; set; }

    }
}

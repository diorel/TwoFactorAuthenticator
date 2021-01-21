using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTwoFactorAuthentication.Models
{
    public class DataGoogle
    {
        public string Emisor { get; set; }
        public string Cuenta { get; set; } 
        public int QRTamaño { get; set; }
    }
}

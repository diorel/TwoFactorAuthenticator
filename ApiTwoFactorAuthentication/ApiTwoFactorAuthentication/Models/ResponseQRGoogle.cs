﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTwoFactorAuthentication.Models
{
    public class ResponseQRGoogle
    {
        public string LlaveSecreta { get; set; }
        public string QRImagen { get; set; }
    }
}

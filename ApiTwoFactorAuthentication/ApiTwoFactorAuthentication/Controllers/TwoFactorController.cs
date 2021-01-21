using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiTwoFactorAuthentication.Models;
using Google.Authenticator;
using TwoStepsAuthenticator;



namespace ApiTwoFactorAuthentication.Controllers
{

    [ApiController]
    public class TwoFactorController : ControllerBase
    {
        [Route("API/V1/Google/GetCode")]
        [HttpPost]
        public ReponseApi GetCodeGoogle(DataGoogle par)
        {
            ReponseApi res = new ReponseApi();
            

            res.Codigo = 0;
            res.Mensaje = "";
            res.Respuesta = null;

            if (string.IsNullOrEmpty(par.Emisor))
            {
                res.Mensaje = "El Emisor no puede ser nulo";
       
            }
            else if(string.IsNullOrEmpty(par.Cuenta))
            {
                res.Mensaje = "La Cuenta no puede ser nula";
            }
            else
            {
                TwoFactorAuthenticator autenticador = new TwoFactorAuthenticator();
                ResponseQRGoogle resGoogle = new ResponseQRGoogle();
                var key = TwoStepsAuthenticator.Authenticator.GenerateKey();

                var setupInfo = autenticador.GenerateSetupCode(par.Emisor, par.Cuenta, key, true, par.QRTamaño);
          
                resGoogle.QRImagen = setupInfo.QrCodeSetupImageUrl;
                resGoogle.LlaveSecreta = setupInfo.ManualEntryKey;

                res.Codigo = 1;
                res.Respuesta = resGoogle;
            }

            return res;
        }



    }
}

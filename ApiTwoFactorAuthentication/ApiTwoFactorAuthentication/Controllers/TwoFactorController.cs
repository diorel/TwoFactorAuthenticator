using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiTwoFactorAuthentication.Models;
using Google.Authenticator;
using TwoStepsAuthenticator;
using ApiTwoFactorAuthentication.Helpers;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using EncodeDotNet;
using Wiry.Base32;



namespace ApiTwoFactorAuthentication.Controllers
{

    [ApiController]
    public class TwoFactorController : ControllerBase
    {
        /// <summary>
        /// Método para obtener clave y código QR Google
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>

        [Route("api/v1/Google/GetCode")]
        [HttpPost]
        public ReponseApi GetCodeGoogle(DataGoogle par)
        {
            ReponseApi res = new ReponseApi();

            res.Codigo = 0;
            res.Mensaje = "";
            res.Respuesta = null;

            try
            {
                if (string.IsNullOrEmpty(par.Emisor))
                {
                    res.Mensaje = "El Emisor no puede ser nulo";

                }
                else if (string.IsNullOrEmpty(par.Cuenta))
                {
                    res.Mensaje = "La Cuenta no puede ser nula";
                }
                else
                {
                    TwoFactorAuthenticator autenticador = new TwoFactorAuthenticator();
                    ResponseQRGoogle resGoogle = new ResponseQRGoogle();
                    var key = TwoStepsAuthenticator.Authenticator.GenerateKey();
                    var setupInfo = autenticador.GenerateSetupCode(par.Emisor, par.Cuenta, key, false, par.QRTamaño <= 0 ? 4 : par.QRTamaño);

                    resGoogle.QRImagen = setupInfo.QrCodeSetupImageUrl;
                    resGoogle.LlaveSecreta = key;
                    resGoogle.CodigoManual = setupInfo.ManualEntryKey;

                    res.Codigo = 1;
                    res.Respuesta = resGoogle;
                }
            } catch(Exception ex)
            {
                //Log Error

            }

            
            return res;
        }

        /// <summary>
        /// Método para verificar código con dispositivo a authenticator google
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>

        [Route("api/v1/Google/ValidateCode")]
        [HttpPost]
        public ReponseApi ValidateCodeGoogle(DataValidateGoogle par)
        {
            ReponseApi res = new ReponseApi();

            res.Codigo = 0;
            res.Mensaje = "";
            res.Respuesta = null;


            if (string.IsNullOrEmpty(par.Cuenta))
            {
                res.Mensaje = "La Cuenta no puede ser nula";

            }
            else if (string.IsNullOrEmpty(par.Emisor))
            {
                res.Mensaje = "El Emisor no puede ser nulo";
            }
            else if (string.IsNullOrEmpty(par.Codigo))
            {
                res.Mensaje = "El Codigo no puede ser nulo";
            }
            else 
            {
                // registrar validar dos factores
                TwoFactorAuthenticator autenticador = new TwoFactorAuthenticator();
                bool PinOK = autenticador.ValidateTwoFactorPIN(par.Cuenta, par.Codigo);

                if (PinOK != true)
                {
                    res.Mensaje = "El Código ingresado no es correcto";
                }
                else
                {
                    res.Codigo = 1;
                    res.Mensaje = "Código Válido";
                }
            }
            return res ;
        }

        /// <summary>
        /// Método para obtener clave y código QR Microsoft
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>

        [Route("api/v1/Microsoft/GetCode")]
        [HttpPost]
        public ReponseApi GetCodeMicrosoft(DataMicrosoft par)
        {
            ReponseApi res = new ReponseApi();
            QRFeatures funciones = new QRFeatures();
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            


            res.Codigo = 0;
            res.Mensaje = "";
            res.Respuesta = null;

            if (string.IsNullOrEmpty(par.Emisor))
            {
                res.Mensaje = "El Emisor no puede ser nulo";

            }
            else if (string.IsNullOrEmpty(par.Cuenta))
            {
                res.Mensaje = "La Cuenta no puede ser nula";
            }
            else 
            {
                ResponseQRMicrosoft resMicrosoft = new ResponseQRMicrosoft();

                var key = TwoStepsAuthenticator.Authenticator.GenerateKey();
                byte[] inputBytes = Encoding.ASCII.GetBytes(key);
                string base32 = Wiry.Base32.Base32Encoding.Standard.GetString(inputBytes);
                string codigoManual = base32.Replace("=", null);
                var AuthenticatorUri = funciones.GenerateQrCodeUri(par.Cuenta,par.Emisor, key);
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(AuthenticatorUri, QRCodeGenerator.ECCLevel.Q);


                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(4);
           
                System.IO.MemoryStream ms = new MemoryStream();
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                var SigBase64 = Convert.ToBase64String(byteImage);

                resMicrosoft.LlaveSecreta = key;
                resMicrosoft.CodigoManual = codigoManual;
                resMicrosoft.QRImagen = "data:image/png;base64," + SigBase64;
        

                res.Codigo = 1;
                res.Respuesta = resMicrosoft;


            }
            return res;
        }

        /// <summary>
        /// Método para verificar código con dispositivo authenticator Microsoft
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>

        [Route("api/v1/Microsoft/ValidateCode")]
        [HttpPost]
        public ReponseApi ValidateCodeMicrosoft(DataValidateMicrosoft par)
        {
            ReponseApi res = new ReponseApi();

            res.Codigo = 0;
            res.Mensaje = "";
            res.Respuesta = null;


            if (string.IsNullOrEmpty(par.Cuenta))
            {
                res.Mensaje = "La Cuenta no puede ser nula";

            }
            else if (string.IsNullOrEmpty(par.Secreto))
            {
                res.Mensaje = "El Emisor no puede ser nulo";
            }
            else if (string.IsNullOrEmpty(par.Codigo))
            {
                res.Mensaje = "El Codigo no puede ser nulo";
            }
            else
            {
                var authenticator = new TwoStepsAuthenticator.TimeAuthenticator();

                bool isok = authenticator.CheckCode(par.Secreto,  par.Codigo , par.Cuenta);

                if (isok != true)
                {
                    res.Mensaje = "El Código ingresado no es correcto";
                }
                else
                {
                    res.Codigo = 1;
                    res.Mensaje = "Código Válido";
                }
            }
            return res;
        }
    }
}

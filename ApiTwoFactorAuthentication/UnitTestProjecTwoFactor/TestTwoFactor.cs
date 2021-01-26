using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ApiTwoFactorAuthentication.Controllers;
using ApiTwoFactorAuthentication.Models;

namespace UnitTestProjecTwoFactor
{
    [TestClass]
    class TestTwoFactor
    {
        [TestMethod]
       public void PruevaGenerarQRGoogle() 
       {
                TwoFactorController metodos = new TwoFactorController();
                DataGoogle datosGoogle = new DataGoogle();
                // Arrange
                int resultadoEsperado = 1;
                datosGoogle.Cuenta  = "correo1@gmail.com";
                datosGoogle.Emisor = "SucursalDigital";
                datosGoogle.QRTamaño = 4;
                //Act
                var resultadoCodigo = metodos.GetCodeGoogle(datosGoogle);
                // Assert
                Assert.Equals(resultadoEsperado, resultadoCodigo);         
        }

        [TestMethod]
        public void PruevaGenerarQRMicrosoft()
        {
            TwoFactorController metodos = new TwoFactorController();
            DataMicrosoft datosMicrosoft = new DataMicrosoft();
            // Arrange
            int resultadoEsperado = 1;
            datosMicrosoft.Cuenta = "correo1@gmail.com";
            datosMicrosoft.Emisor = "SucursalDigital";
            //Act
            var resultadoCodigo = metodos.GetCodeMicrosoft(datosMicrosoft);
            // Assert
            Assert.Equals(resultadoEsperado, resultadoCodigo);
        }


    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiTwoFactorAuthentication.Controllers;
using ApiTwoFactorAuthentication.Models;
namespace UnitTestProjecTwoFactor
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PruevaGenerarQRGoogle()
        {
            TwoFactorController metodos = new TwoFactorController();
            DataGoogle datosGoogle = new DataGoogle();
            // Arrange
            string resultadoEsperado = "1";
            datosGoogle.Cuenta = "correo1@gmail.com";
            datosGoogle.Emisor = "SucursalDigital";
            datosGoogle.QRTamaño = 4;

            //Act
            var resultadoCodigo = metodos.GetCodeGoogle(datosGoogle);
            string resultadoMetodo = resultadoCodigo.Codigo.ToString();

            // Assert
            Assert.AreEqual(resultadoEsperado, resultadoMetodo);
        }


        [TestMethod]
        public void PruevaGenerarQRMicrosoft()
        {
            TwoFactorController metodos = new TwoFactorController();
            DataGoogle datosGoogle = new DataGoogle();
            // Arrange
            string resultadoEsperado = "1";
            datosGoogle.Cuenta = "correo1@gmail.com";
            datosGoogle.Emisor = "SucursalDigital";
            datosGoogle.QRTamaño = 4;

            //Act
            var resultadoCodigo = metodos.GetCodeGoogle(datosGoogle);
            string resultadoMetodo = resultadoCodigo.Codigo.ToString();

            // Assert
            Assert.AreEqual(resultadoEsperado, resultadoMetodo);
        }
    }
}

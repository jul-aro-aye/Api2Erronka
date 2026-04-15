using ErronkaApi.DTOak;
using ErronkaApi.Modeloak;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using ErronkaApi.Kontrollerrak;
using ErronkaApi.Repositorioak;

namespace ErronkaApi.Testak
{
    
    using System;
    public class LoginKontrollerTestak
    {

        [Fact]
        public void Login_DatuZuzenekin_OkEtaErabiltzailearenDatuakItzuliBeharDitu()
        {
            // Arrange
            var erabiltzailea = new Erabiltzailea
            {
                id = 1,
                erabiltzailea = "jon",
                emaila = "jon@test.com",
                pasahitza = "1234",
                rola = new Rola { id = 1, izena = "admin" },
                ezabatua = false,
                txat = true
            };

            var mockRepo = new Mock<ErabiltzaileaRepository>();

            mockRepo.Setup(r => r.Login("jon", "1234"))
                    .Returns((true, null, erabiltzailea));

            var controller = new LoginKontrollera(mockRepo.Object);

            var dto = new LoginDTO
            {
                erabiltzailea = "jon",
                pasahitza = "1234"
            };

            // Act
            var result = controller.Login(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<object>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Login egokia", body.Message);

            var lista = Assert.IsType<List<object>>(body.Datuak);
            Assert.Single(lista);

            dynamic datuak = lista[0];

            Assert.Equal(1, datuak.id);
            Assert.Equal("jon", datuak.erabiltzailea);
            Assert.Equal("jon@test.com", datuak.emaila);
            Assert.Equal("admin", datuak.rola.izena);
            Assert.True(datuak.txat);
        }

        [Fact]
        public void Login_ErabiltzaileEdoPasahitzOkerrarekin_UnauthorizedItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErabiltzaileaRepository>();

            mockRepo.Setup(r => r.Login("erabiltzailea", "Gaizki"))
                    .Returns((false, "Erabiltzailea edo pasahitza okerra", null as Erabiltzailea));

            var controller = new LoginKontrollera(mockRepo.Object);

            var dto = new LoginDTO
            {
                erabiltzailea = "erabiltzailea",
                pasahitza = "Gaizki"
            };

            // Act
            var result = controller.Login(dto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(unauthorized.Value);

            Assert.Equal(401, body.Code);
            Assert.Equal("Erabiltzailea edo pasahitza okerra", body.Message);
            Assert.Null(body.Datuak);
        }

        [Fact]
        public void LortuTxatBaimena_ErabiltzaileaExistitzenBada_OkItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErabiltzaileaRepository>();
            mockRepo.Setup(r => r.LortuTxatBaimena(4))
                    .Returns((true, true, null, true));

            var controller = new LoginKontrollera(mockRepo.Object);

            // Act
            var result = controller.LortuTxatBaimena(4);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<bool>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Txat baimena lortu da", body.Message);
            Assert.NotNull(body.Datuak);
            Assert.Single(body.Datuak);
            Assert.True(body.Datuak![0]);
        }

        [Fact]
        public void LortuTxatBaimena_ErabiltzaileaEzBadago_NotFoundItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErabiltzaileaRepository>();
            mockRepo.Setup(r => r.LortuTxatBaimena(99))
                    .Returns((true, false, null, false));

            var controller = new LoginKontrollera(mockRepo.Object);

            // Act
            var result = controller.LortuTxatBaimena(99);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(notFound.Value);

            Assert.Equal(404, body.Code);
            Assert.Equal("Erabiltzailea ez da aurkitu", body.Message);
        }

        [Fact]
        public void LortuTxatBaimena_ErroreaBadago_500ItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErabiltzaileaRepository>();
            mockRepo.Setup(r => r.LortuTxatBaimena(4))
                    .Returns((false, false, "DB errorea", false));

            var controller = new LoginKontrollera(mockRepo.Object);

            // Act
            var result = controller.LortuTxatBaimena(4);

            // Assert
            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);

            var body = Assert.IsType<ErantzunaDTO<string>>(error.Value);
            Assert.Equal(500, body.Code);
            Assert.Equal("DB errorea", body.Message);
        }
    }
}

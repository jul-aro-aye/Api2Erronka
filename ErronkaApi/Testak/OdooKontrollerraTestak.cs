using ErronkaApi.DTOak;
using ErronkaApi.Kontrollerrak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHibernate;
using Xunit;

namespace ErronkaApi.Testak
{
    public class OdooKontrollerraTestak
    {
        [Fact]
        public void LortuSinkronizazioDatuak_ErroreaBadago_500ItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<OdooRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuSinkronizazioDatuak())
                    .Returns((false, "DB errorea", null as OdooSyncDTO));

            var controller = new OdooKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuSinkronizazioDatuak();

            // Assert
            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);

            var body = Assert.IsType<ErantzunaDTO<string>>(error.Value);
            Assert.Equal(500, body.Code);
            Assert.Equal("DB errorea", body.Message);
            Assert.Null(body.Datuak);
        }

        [Fact]
        public void LortuSinkronizazioDatuak_DatuakDaudenean_OkItzuliBeharDu()
        {
            // Arrange
            var data = new OdooSyncDTO
            {
                zerbitzariak = new List<OdooZerbitzariaDTO>
                {
                    new OdooZerbitzariaDTO
                    {
                        id = 1,
                        izena = "ane",
                        emaila = "ane@test.com",
                        rolaIzena = "zerbitzaria",
                        txat = true
                    }
                }
            };

            var mockRepo = new Mock<OdooRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuSinkronizazioDatuak())
                    .Returns((true, null, data));

            var controller = new OdooKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuSinkronizazioDatuak();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<OdooSyncDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Odoorako sinkronizazio datuak eskuratu dira", body.Message);
            Assert.Single(body.Datuak!);
            Assert.Single(body.Datuak!.First().zerbitzariak);
        }
    }
}

using ErronkaApi.DTOak;
using ErronkaApi.Kontrollerrak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHibernate;
using Xunit;

namespace ErronkaApi.Testak
{
    public class ErreserbaMahaiakKontrollerraTestak
    {
        [Fact]
        public void GehituMahaiErreserbara_ErroreaBadago_BadRequestItzuliBeharDu()
        {
            // Arrange
            var dto = new ErreserbaMahaiDTO { ErreserbakId = 1, MahaiakId = 2 };
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EzarriMahaia(1, 2))
                    .Returns((false, "Mahaia ez da aurkitu", null as int?));

            var controller = new ErreserbaMahaiakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.GehituMahaiErreserbara(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(badRequest.Value);

            Assert.Equal(400, body.Code);
            Assert.Equal("Mahaia ez da aurkitu", body.Message);
        }

        [Fact]
        public void GehituMahaiErreserbara_DatuZuzenekin_OkItzuliBeharDu()
        {
            // Arrange
            var dto = new ErreserbaMahaiDTO { ErreserbakId = 1, MahaiakId = 2 };
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EzarriMahaia(1, 2))
                    .Returns((true, null, 2));

            var controller = new ErreserbaMahaiakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.GehituMahaiErreserbara(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<int>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Mahaia erreserbara gehitu da", body.Message);
            Assert.Single(body.Datuak!);
            Assert.Equal(2, body.Datuak!.First());
        }

        [Fact]
        public void LortuMahaiakErreserbarentzat_EzBadaExistitzen_NotFoundItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuMahaiakErreserbarentzat(1))
                    .Returns((false, "Erreserba ez da aurkitu", null as List<int>));

            var controller = new ErreserbaMahaiakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuMahaiakErreserbarentzat(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(notFound.Value);

            Assert.Equal(404, body.Code);
            Assert.Equal("Erreserba ez da aurkitu", body.Message);
        }

        [Fact]
        public void LortuMahaiakErreserbarentzat_ExistitzenBada_OkItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuMahaiakErreserbarentzat(1))
                    .Returns((true, null, new List<int> { 2 }));

            var controller = new ErreserbaMahaiakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuMahaiakErreserbarentzat(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<int>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Erreserbako mahaiak lortu dira", body.Message);
            Assert.Single(body.Datuak!);
            Assert.Equal(2, body.Datuak!.First());
        }

        [Fact]
        public void EzabatuMahaiakErreserbatik_OkEtaMezuaItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            var controller = new ErreserbaMahaiakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EzabatuMahaiakErreserbatik(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Ez da aldaketarik egin", body.Message);
        }
    }
}

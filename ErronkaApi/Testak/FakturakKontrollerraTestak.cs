using ErronkaApi.DTOak;
using ErronkaApi.Kontrollerrak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHibernate;
using Xunit;

namespace ErronkaApi.Testak
{
    public class FakturakKontrollerraTestak
    {
        [Fact]
        public void LortuFakturak_ErroreaBadago_500ItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuFakturak())
                    .Returns((false, "DB errorea", null as List<FakturaDTO>));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuFakturak();

            // Assert
            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);

            var body = Assert.IsType<ErantzunaDTO<string>>(error.Value);
            Assert.Equal(500, body.Code);
            Assert.Equal("DB errorea", body.Message);
        }

        [Fact]
        public void LortuFakturak_DatuakDaudenean_OkItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuFakturak())
                    .Returns((true, null, new List<FakturaDTO> { SortuFakturaDto() }));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuFakturak();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<FakturaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Fakturak lortu dira", body.Message);
            Assert.Single(body.Datuak!);
        }

        [Fact]
        public void LortuFaktura_EzBadaExistitzen_NotFoundItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuFaktura(1))
                    .Returns((false, "Faktura ez da aurkitu", null as FakturaDTO));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuFaktura(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(notFound.Value);

            Assert.Equal(404, body.Code);
            Assert.Equal("Faktura ez da aurkitu", body.Message);
        }

        [Fact]
        public void LortuFaktura_ExistitzenBada_OkItzuliBeharDu()
        {
            // Arrange
            var faktura = SortuFakturaDto();
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuFaktura(1))
                    .Returns((true, null, faktura));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuFaktura(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<FakturaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Faktura lortu da", body.Message);
            Assert.Single(body.Datuak!);
            Assert.Equal(1, body.Datuak!.First().Id);
        }

        [Fact]
        public void LortuFakturaErreserbarenArabera_EzBadago_NotFoundItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuFakturaErreserbarenArabera(7))
                    .Returns((false, "Erreserba horretarako fakturarik ez dago", null as FakturaDTO));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuFakturaErreserbarenArabera(7);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void LortuFakturaErreserbarenArabera_Badago_OkItzuliBeharDu()
        {
            // Arrange
            var faktura = SortuFakturaDto();
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuFakturaErreserbarenArabera(7))
                    .Returns((true, null, faktura));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuFakturaErreserbarenArabera(7);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<FakturaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Faktura lortu da", body.Message);
            Assert.Single(body.Datuak!);
        }

        [Fact]
        public void SortuEdoLortuFakturaErreserbatik_ErroreaBadago_BadRequestItzuliBeharDu()
        {
            // Arrange
            var dto = new FakturaErreserbaSortuDTO { ErreserbaId = 7 };
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.SortuEdoLortuFakturaErreserbatik(7))
                    .Returns((false, "Erreserba horri lotutako eskaerarik ez dago", null as FakturaDTO));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.SortuEdoLortuFakturaErreserbatik(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(badRequest.Value);

            Assert.Equal(400, body.Code);
            Assert.Equal("Erreserba horri lotutako eskaerarik ez dago", body.Message);
        }

        [Fact]
        public void SortuEdoLortuFakturaErreserbatik_DatuZuzenekin_OkItzuliBeharDu()
        {
            // Arrange
            var dto = new FakturaErreserbaSortuDTO { ErreserbaId = 7 };
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.SortuEdoLortuFakturaErreserbatik(7))
                    .Returns((true, null, SortuFakturaDto()));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.SortuEdoLortuFakturaErreserbatik(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<FakturaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Faktura prest dago", body.Message);
            Assert.Single(body.Datuak!);
        }

        [Fact]
        public void EzabatuFaktura_EzBadaExistitzen_NotFoundItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EzabatuFaktura(1))
                    .Returns((false, "Faktura ez da aurkitu"));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EzabatuFaktura(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(notFound.Value);

            Assert.Equal(404, body.Code);
            Assert.Equal("Faktura ez da aurkitu", body.Message);
        }

        [Fact]
        public void EzabatuFaktura_ExistitzenBada_OkItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EzabatuFaktura(1))
                    .Returns((true, null));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EzabatuFaktura(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Faktura ezabatu da", body.Message);
        }

        [Fact]
        public void EguneratuTotala_ErroreaBadago_BadRequestItzuliBeharDu()
        {
            // Arrange
            var dto = new FakturaTotalaEguneratuDTO { FakturaId = 1, Gehikuntza = 5.5m };
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EguneratuTotala(1, 5.5m))
                    .Returns((false, "Faktura ez da aurkitu", null as FakturaDTO));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EguneratuTotala(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(badRequest.Value);

            Assert.Equal(400, body.Code);
            Assert.Equal("Faktura ez da aurkitu", body.Message);
        }

        [Fact]
        public void EguneratuTotala_DatuZuzenekin_OkItzuliBeharDu()
        {
            // Arrange
            var dto = new FakturaTotalaEguneratuDTO { FakturaId = 1, Gehikuntza = 5.5m };
            var mockRepo = new Mock<FakturaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EguneratuTotala(1, 5.5m))
                    .Returns((true, null, SortuFakturaDto()));

            var controller = new FakturakKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EguneratuTotala(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<FakturaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Fakturaren totala eguneratu da", body.Message);
            Assert.Single(body.Datuak!);
        }

        private static FakturaDTO SortuFakturaDto()
        {
            return new FakturaDTO
            {
                Id = 1,
                EskaeraId = 3,
                ErreserbaId = 7,
                PdfIzena = "faktura.pdf",
                Data = new DateTime(2026, 4, 30),
                Totala = 25.5m
            };
        }
    }
}

using ErronkaApi.DTOak;
using ErronkaApi.Kontrollerrak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHibernate;
using Xunit;

namespace ErronkaApi.Testak
{
    public class ErreserbaKontrollerraTestak
    {
        [Fact]
        public void LortuErreserbak_ErroreaBadago_500ItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuErreserbak())
                    .Returns((false, "DB errorea", null as List<ErreserbaDTO>));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuErreserbak();

            // Assert
            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);

            var body = Assert.IsType<ErantzunaDTO<string>>(error.Value);
            Assert.Equal(500, body.Code);
            Assert.Equal("DB errorea", body.Message);
        }

        [Fact]
        public void LortuErreserbak_DatuakDaudenean_OkItzuliBeharDu()
        {
            // Arrange
            var data = new List<ErreserbaDTO> { SortuErreserbaDto() };
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuErreserbak())
                    .Returns((true, null, data));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuErreserbak();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<ErreserbaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Erreserbak lortu dira", body.Message);
            Assert.Single(body.Datuak!);
        }

        [Fact]
        public void LortuErreserbakData_DataOkerrarekin_BadRequestItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuErreserbakData("ez-da-data");

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(badRequest.Value);

            Assert.Equal(400, body.Code);
            Assert.Equal("Data ez da zuzena", body.Message);
        }

        [Fact]
        public void LortuErreserbakData_RepoErrorearekin_500ItzuliBeharDu()
        {
            // Arrange
            var data = new DateTime(2026, 4, 30);
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuErreserbakDatarenArabera(data))
                    .Returns((false, "DB errorea", null as List<ErreserbaDTO>));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuErreserbakData("2026-04-30");

            // Assert
            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }

        [Fact]
        public void LortuErreserbakData_DataZuzenarekin_OkItzuliBeharDu()
        {
            // Arrange
            var data = new DateTime(2026, 4, 30);
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.LortuErreserbakDatarenArabera(data))
                    .Returns((true, null, new List<ErreserbaDTO> { SortuErreserbaDto() }));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.LortuErreserbakData("2026-04-30");

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<ErreserbaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Erreserbak lortu dira", body.Message);
            Assert.Single(body.Datuak!);
        }

        [Fact]
        public void SortuErreserba_DatuOkerrekin_BadRequestItzuliBeharDu()
        {
            // Arrange
            var dto = SortuErreserbaDto();
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.SortuErreserba(dto))
                    .Returns((false, "Mahaia beharrezkoa da", null as ErreserbaDTO));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.SortuErreserba(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(badRequest.Value);

            Assert.Equal(400, body.Code);
            Assert.Equal("Mahaia beharrezkoa da", body.Message);
        }

        [Fact]
        public void SortuErreserba_DatuZuzenekin_OkItzuliBeharDu()
        {
            // Arrange
            var dto = SortuErreserbaDto();
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.SortuErreserba(dto))
                    .Returns((true, null, dto));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.SortuErreserba(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<ErreserbaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Erreserba sortu da", body.Message);
            Assert.Single(body.Datuak!);
        }

        [Fact]
        public void EguneratuErreserba_ErroreaBadago_BadRequestItzuliBeharDu()
        {
            // Arrange
            var dto = SortuErreserbaDto();
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EguneratuErreserba(1, dto))
                    .Returns((false, "Erreserba ez da aurkitu", null as ErreserbaDTO));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EguneratuErreserba(1, dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(badRequest.Value);

            Assert.Equal(400, body.Code);
            Assert.Equal("Erreserba ez da aurkitu", body.Message);
        }

        [Fact]
        public void EguneratuErreserba_DatuZuzenekin_OkItzuliBeharDu()
        {
            // Arrange
            var dto = SortuErreserbaDto();
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EguneratuErreserba(1, dto))
                    .Returns((true, null, dto));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EguneratuErreserba(1, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<ErreserbaDTO>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Erreserba eguneratu da", body.Message);
            Assert.Single(body.Datuak!);
        }

        [Fact]
        public void EzabatuErreserba_EzBadaExistitzen_NotFoundItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EzabatuErreserba(1))
                    .Returns((false, "Erreserba ez da aurkitu"));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EzabatuErreserba(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(notFound.Value);

            Assert.Equal(404, body.Code);
            Assert.Equal("Erreserba ez da aurkitu", body.Message);
        }

        [Fact]
        public void EzabatuErreserba_ExistitzenBada_OkItzuliBeharDu()
        {
            // Arrange
            var mockRepo = new Mock<ErreserbaRepository>(Mock.Of<ISessionFactory>());
            mockRepo.Setup(r => r.EzabatuErreserba(1))
                    .Returns((true, null));

            var controller = new ErreserbaKontrollerra(mockRepo.Object);

            // Act
            var result = controller.EzabatuErreserba(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<ErantzunaDTO<string>>(ok.Value);

            Assert.Equal(200, body.Code);
            Assert.Equal("Erreserba ezabatu da", body.Message);
        }

        private static ErreserbaDTO SortuErreserbaDto()
        {
            return new ErreserbaDTO
            {
                Id = 1,
                MahaiaId = 2,
                Izena = "Ane",
                Telefonoa = "600000000",
                Txanda = "Bazkaria",
                PertsonaKopurua = 4,
                Data = new DateTime(2026, 4, 30),
                Egoera = "sortua"
            };
        }
    }
}

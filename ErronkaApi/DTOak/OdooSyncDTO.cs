namespace ErronkaApi.DTOak
{
    public class OdooSyncDTO
    {
        public List<OdooZerbitzariaDTO> zerbitzariak { get; set; } = new();
        public List<OdooProduktuaDTO> platerak { get; set; } = new();
        public List<OdooMahaiaDTO> mahaiak { get; set; } = new();
        public List<OdooEskaeraDTO> eskaerak { get; set; } = new();
        public List<FakturaDTO> fakturak { get; set; } = new();
    }

    public class OdooZerbitzariaDTO
    {
        public int id { get; set; }
        public string izena { get; set; } = string.Empty;
        public string emaila { get; set; } = string.Empty;
        public string rolaIzena { get; set; } = string.Empty;
        public bool txat { get; set; }
    }

    public class OdooProduktuaDTO
    {
        public int id { get; set; }
        public string izena { get; set; } = string.Empty;
        public decimal prezioa { get; set; }
        public int kategoriaId { get; set; }
        public int stockAktuala { get; set; }
    }

    public class OdooMahaiaDTO
    {
        public int id { get; set; }
        public int zenbakia { get; set; }
        public int kapazitatea { get; set; }
        public string egoera { get; set; } = string.Empty;
    }

    public class OdooEskaeraDTO
    {
        public int id { get; set; }
        public string izena { get; set; } = string.Empty;
        public int mahaiaId { get; set; }
        public int erabiltzaileId { get; set; }
        public string erabiltzaileIzena { get; set; } = string.Empty;
        public int komensalak { get; set; }
        public string egoera { get; set; } = string.Empty;
        public string sukaldeaEgoera { get; set; } = string.Empty;
        public DateTime sortzeData { get; set; }
        public DateTime? itxieraData { get; set; }
        public string txanda { get; set; } = string.Empty;
        public List<OdooEskaeraLerroDTO> lerroak { get; set; } = new();
    }

    public class OdooEskaeraLerroDTO
    {
        public int id { get; set; }
        public int produktuaId { get; set; }
        public string produktuaIzena { get; set; } = string.Empty;
        public int kantitatea { get; set; }
        public decimal prezioUnitarioa { get; set; }
        public decimal guztira { get; set; }
    }
}

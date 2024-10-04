namespace ExchangeRatesAPI.Models
{
    public class ExchangeRate
    {
        public string Code { get; set; }
        public string NameTR { get; set; }
        public string NameEN { get; set; }
        public double Rate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}

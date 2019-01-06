namespace CoinMonitor.Domain.Dtos.Cex
{
	public class CurrencyLimitDto
	{
		public string Symbol1 { get; set; }
		public string Symbol2 { get; set; }
		public decimal MinLotSize { get; set; }
		public decimal MinLotSizeS2 { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
	}
}

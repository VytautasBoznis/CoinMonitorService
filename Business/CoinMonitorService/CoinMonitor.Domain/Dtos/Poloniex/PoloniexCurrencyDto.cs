namespace CoinMonitor.Domain.Dtos.Poloniex
{
	public class PoloniexCurrencyDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string HumanType { get; set; }
		public decimal TxFee { get; set; }
		public int MinConf { get; set; }
		public string DepositAddress { get; set; }
		public bool Disabled { get; set; }
		public bool Delisted { get; set; }
		public bool Frozen { get; set; }
	}
}

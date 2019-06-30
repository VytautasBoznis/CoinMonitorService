using CoinMonitor.Domain.Messages.Base;

namespace CoinMonitor.Domain.Dtos.Poloniex
{
	public class PoloniexCurrencyDto: BaseRestResponse
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal last { get; set; }
		public decimal lowestAsk { get; set; }
		public decimal highestBid { get; set; }
		public decimal percentChange { get; set; }
		public decimal baseVolume { get; set; }
		public decimal quoteVolume { get; set; }
		public decimal isFrozen { get; set; }
		public decimal high24hr { get; set; }
		public decimal low24hr { get; set; }
	}
}

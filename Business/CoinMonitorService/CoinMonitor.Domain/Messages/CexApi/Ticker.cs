using System;
using CoinMonitor.Domain.Messages.Base;

namespace CoinMonitor.Domain.Messages.CexApi
{
	public class TickerRequest : BaseCexRequest
	{
	}

	public class TickerResponse : BaseRestResponse 
	{
		public long Timestamp { get; set; }
		public decimal Low { get; set; }
		public decimal High { get; set; }
		public decimal Last { get; set; }
		public decimal Volume { get; set; }
		public decimal Volume30D { get; set; }
		public decimal Bid { get; set; }
		public decimal Ask { get; set; }
		public string PriceChange { get; set; }
		public string PriceChangePercentage { get; set; }
		public string Pair { get; set; }
		public DateTime TimestampDate => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(Timestamp);
	}
}

using System;
using CoinMonitor.Domain.Enums;

namespace CoinMonitor.Domain.Dtos.EcoIndex
{
	public class TickerFormattedDto
	{
		public ExchangeTypeEnum ExchangeType { get; set; }
		public ExchangePairTypeEnum PairType { get; set; }
		public DateTime Time { get; set; }
		public decimal Low { get; set; }
		public decimal High { get; set; }
		public decimal Last { get; set; }
		public decimal Volume { get; set; }
	}
}

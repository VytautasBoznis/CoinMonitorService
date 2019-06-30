using System;

namespace CoinMonitor.Domain.Dtos.EcoIndex.Formatting
{
	public class TickerResponseFormattedDto
	{
		public short ExchangeType;
		public short PairType;
		public DateTime Time;
		public decimal Low;
		public decimal High;
		public decimal Last;
		public decimal Volume;
	}
}

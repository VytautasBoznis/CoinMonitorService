using System;

namespace CoinMonitor.RestClients.Holders
{
	public static class ElasticApiUrlHolder
	{
		public enum ExchangeName
		{
			Test,
			CEX,
			POLONIEX
		}

		public enum DataType
		{
			RAW,
			FORMATTED,
			ECO_INDEX
		}

		public static string GenerateDataSaveUrl(ExchangeName excName, DataType dataType, string pair, DateTime time)
		{
			return "/" + excName.ToString().ToLower() + "_" + dataType.ToString().ToLower() + "/" + excName.ToString().ToLower() + "/" + pair.ToLower() + "_" + time.Ticks;
		}
	}

	
}

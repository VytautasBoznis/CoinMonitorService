using System;

namespace CoinMonitor.RestClients.Holders
{
	public static class ElasticApiUrlHolder
	{
		public enum ExchangeName
		{
			CEX,
			POLONIEX
		}

		public enum DataType
		{
			RAW,
			SANITIZED,
			ECO_INDEX
		}

		public static string GenerateDataSaveUrl(ExchangeName excName, DataType dataType, string pair)
		{
			return "/" + excName.ToString().ToLower() + "/" + dataType.ToString().ToLower() + "/" + pair.ToLower() + "_" + DateTime.Now.Ticks;
		}
	}

	
}

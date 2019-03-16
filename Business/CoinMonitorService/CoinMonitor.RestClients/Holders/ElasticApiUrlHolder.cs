namespace CoinMonitor.RestClients.Holders
{
	public static class ElasticApiUrlHolder
	{
		private static string BaseUrl = "CM";

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

		public static string GenerateDataPutUrl(ExchangeName excName, DataType dataType, string pairName)
		{
			return BaseUrl + "/" + excName + "/" + dataType + "/" + pairName;
		}
	}

	
}

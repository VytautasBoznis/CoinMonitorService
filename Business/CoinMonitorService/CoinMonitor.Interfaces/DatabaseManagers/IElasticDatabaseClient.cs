using System;

namespace CoinMonitor.Interfaces.DatabaseManagers
{
	public interface IElasticDatabaseClient
	{
		void SaveRawTickerData(string pair, object tickerResponse, Type responseType);
	}
}

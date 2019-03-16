using CoinMonitor.Domain.Dtos.Cex;

namespace CoinMonitor.Interfaces.DatabaseManagers
{
	public interface IElasticDatabaseClient
	{
		void SaveRawTickerData(string pair, object tickerResponse);
	}
}

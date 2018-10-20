using CoinMonitor.Business.Managers.Base;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.RestClients.Clients;

namespace CoinMonitor.Business.Managers.Market
{
	public class CexMarketManager: MarketWatchManager
	{
		private readonly CexApiClient _cexApiClient; 

		public CexMarketManager()
		{
			_cexApiClient = new CexApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.CexApiUrl, ""));
		}

		//will get all possible market data from the rest client
		public override void GetMarketData()
		{
			Logger.DebugFormat("Trying to get market data from Cex API");
		}

		//will get all pair historic data from the rest api client
		public override void GetPairHistory()
		{
			Logger.DebugFormat("Trying to get pair history from Cex API");
		}
	}
}

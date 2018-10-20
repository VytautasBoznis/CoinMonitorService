using CoinMonitor.Business.Managers.Base;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.Interfaces.OutsideApiManagers;
using CoinMonitor.RestClients.Clients;

namespace CoinMonitor.Business.Managers.Market
{
	public class PoloniexMarketManager: MarketWatchManager, IPoloniexApiManager
	{
		private readonly PoloniexApiClient _poloniexApiClient;

		public PoloniexMarketManager()
		{
			_poloniexApiClient = new PoloniexApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.PoloniexApiUrl, ""));
		}

		//will get all possible market data from the rest client
		public override void GetMarketData()
		{
			Logger.DebugFormat("Trying to get market data from Poloniex API");
		}

		//will get all pair historic data from the rest api client
		public override void GetPairHistory()
		{
			Logger.DebugFormat("Trying to get pair history from Poloniex API");
		}
	}
}

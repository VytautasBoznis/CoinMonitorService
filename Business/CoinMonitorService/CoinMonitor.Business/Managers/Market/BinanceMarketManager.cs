using CoinMonitor.Business.Managers.Base;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.RestClients.Clients;

namespace CoinMonitor.Business.Managers.Market
{
	public class BinanceMarketManager: MarketWatchManager
	{
		private readonly BinanceApiClient _binanceApiClient;

		public BinanceMarketManager()
		{
			_binanceApiClient = new BinanceApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.BinanceApiUrl, ""));
		}

		//will get all possible market data from the rest client
		public override void GetMarketData()
		{
			Logger.DebugFormat("Trying to get market data from Binance API");
		}

		//will get all pair historic data from the rest api client
		public override void GetPairHistory()
		{
			Logger.DebugFormat("Trying to get pair history from Binance API");
		}
	}
}

using System.Collections.Generic;
using CoinMonitor.Business.Managers.Base;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.Domain.Dtos.Poloniex;
using CoinMonitor.RestClients.Clients;
using Newtonsoft.Json;

namespace CoinMonitor.Business.Managers.Market
{
	public class PoloniexMarketManager: MarketWatchManager
	{
		private readonly PoloniexApiClient _poloniexApiClient;

		public PoloniexMarketManager()
		{
			_poloniexApiClient = new PoloniexApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.PoloniexApiUrl, ""));
		}

		//will get all possible market data from the rest client
		public override void GetMarketData()
		{
			Dictionary<string, PoloniexCurrencyDto> currencies = _poloniexApiClient.GetPoloniexCurrencies();
			Logger.DebugFormat("the full response {0}", JsonConvert.SerializeObject(currencies));
		}

		//will get all pair historic data from the rest api client
		public override void GetPairHistory()
		{
			Logger.DebugFormat("Trying to get pair history from Poloniex API");
		}
	}
}

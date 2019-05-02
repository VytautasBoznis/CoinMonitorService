using System.Collections.Generic;
using System.Web;
using CoinMonitor.Business.Managers.Base;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.Domain.Dtos.Cex;
using CoinMonitor.Domain.Messages.CexApi;
using CoinMonitor.Interfaces.DatabaseManagers;
using CoinMonitor.RestClients.Clients;
using CoinMonitor.RestClients.DatabaseClient;
using CoinMonitor.RestClients.Holders;
using Newtonsoft.Json;

namespace CoinMonitor.Business.Managers.Market
{
	public class CexMarketManager: MarketWatchManager, IDataFormatter
	{
		private readonly ElasticDatabaseClient _elasticClient;
		private readonly CexApiClient _cexApiClient; 

		public CexMarketManager()
		{
			_cexApiClient = new CexApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.CexApiUrl, ""));
			_elasticClient = new ElasticDatabaseClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.ElasticApiUrl, ""), ElasticApiUrlHolder.ExchangeName.CEX);
		}

		//will get all possible market data from the rest client
		public override void GetMarketData()
		{
			//should be user preset
			List<CoinPairDto> coinPair = new List<CoinPairDto>
			{
				new CoinPairDto
				{
					symbol1 = "BTC",
					symbol2 = "USD"
				},
				new CoinPairDto
				{
					symbol1 = "ETH",
					symbol2 = "USD"
				}
			};

			foreach (CoinPairDto pair in coinPair)
			{
				Logger.DebugFormat($"Pair ticker for {pair.symbol1} & {pair.symbol2}");
				TickerResponse response = _cexApiClient.TickPair(pair.symbol1, pair.symbol2);
				Logger.DebugFormat(string.Format("Pair ticker response for {0} & {1} response: {2}", pair.symbol1, pair.symbol2, HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(response))));
				_elasticClient.SaveRawTickerData(GetApiPairName(pair.symbol1, pair.symbol2), response, typeof(TickerResponse));
			}
		}

		//will get all pair historic data from the rest api client
		public override void GetPairHistory()
		{
			Logger.DebugFormat("Trying to get pair history from Cex API");
		}

		#region IDataFormatter implementation

		public string GetApiPairName(string symbol1, string symbol2)
		{
			return symbol1.ToLower() + "_" + symbol2.ToLower();
		}

		#endregion
	}
}

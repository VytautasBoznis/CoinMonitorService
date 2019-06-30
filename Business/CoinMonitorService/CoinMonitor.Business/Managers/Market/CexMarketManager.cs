using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoinMonitor.Business.Managers.Base;
using CoinMonitor.Business.Managers.EcoIndex;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.Domain.Dtos.Cex;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Enums;
using CoinMonitor.Domain.Messages.Base;
using CoinMonitor.Domain.Messages.CexApi;
using CoinMonitor.Domain.Messages.Elastic;
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
		private readonly CoinMonitoringApiClient _coinMonitoringApiClient;
		private readonly CexApiClient _cexApiClient;
		private readonly RSIEcoIndexManager _rsiManager;
		private readonly EMAEcoIndexManager _emaManager;
		private readonly ForceIndexEcoIndexManager _forceIndexEcoIndexManager;

		public CexMarketManager()
		{
			_cexApiClient = new CexApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.CexApiUrl, ""));
			_elasticClient = new ElasticDatabaseClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.ElasticApiUrl, ""), ElasticApiUrlHolder.ExchangeName.CEX);
			_coinMonitoringApiClient = new CoinMonitoringApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.CoinMonitoringApi, ""));
			_rsiManager = new RSIEcoIndexManager(ElasticApiUrlHolder.ExchangeName.CEX);
			_emaManager = new EMAEcoIndexManager(ElasticApiUrlHolder.ExchangeName.CEX);
			_forceIndexEcoIndexManager = new ForceIndexEcoIndexManager(ElasticApiUrlHolder.ExchangeName.CEX);
		}
		
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
				},
				new CoinPairDto
				{
					symbol1 = "ETH",
					symbol2 = "BTC"
				},
				new CoinPairDto
				{
					symbol1 = "ETH",
					symbol2 = "EUR"
				},
				new CoinPairDto
				{
					symbol1 = "BTC",
					symbol2 = "EUR"
				}
			};

			List<TickerResponse> response = _cexApiClient.TickPairs("BTC", "ETH", "USD", "EUR");
			
			foreach (CoinPairDto pair in coinPair)
			{
				TickerResponse ticker = response.FirstOrDefault(t => t.Pair == pair.symbol1 + ":" + pair.symbol2);

				if (ticker != null)
				{
					_elasticClient.SaveRawTickerData(GetApiPairName(pair.symbol1, pair.symbol2), ticker, typeof(TickerResponse), ticker.TimestampDate);
					_elasticClient.SaveSanitizedDate(SanitizeTickerData(ticker, pair));
				}
				
			}

			_rsiManager.CalculateValue();
			_emaManager.CalculateValue();
			_forceIndexEcoIndexManager.CalculateValue();
			_coinMonitoringApiClient.InvokeRecalculation(new BaseRestRequest());
		}
		
		public override void GetPairHistory()
		{
			Logger.DebugFormat("Trying to get pair history from Cex API");
		}

		#region IDataFormatter implementation

		public string GetApiPairName(string symbol1, string symbol2)
		{
			return symbol1.ToLower() + "_" + symbol2.ToLower();
		}

		public TickerFormattedDto SanitizeTickerData(BaseRestResponse response, CoinPairDto pair)
		{
			TickerResponse ticker = (TickerResponse) response;

			if (ticker != null)
			{
				return new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					PairType = ResolvePairType(pair.symbol1, pair.symbol2),
					High = ticker.High,
					Last = ticker.Last,
					Low = ticker.Low,
					Time = ticker.TimestampDate,
					Volume = ticker.Volume
				};
			}

			Logger.ErrorFormat("Failed to convert Cex ticker response to sanitized data the response is null");
			throw new Exception("Failed to convert Cex ticker response to sanitized data the response is null");
		}

		public ExchangePairTypeEnum ResolvePairType(string symbol1, string symbol2)
		{
			switch (symbol1.ToLower())
			{
				case "btc" when symbol2.ToLower() == "usd":
					return ExchangePairTypeEnum.BTC_USD;
				case "eth" when symbol2.ToLower() == "usd":
					return ExchangePairTypeEnum.ETH_USD;
				case "eth" when symbol2.ToLower() == "btc":
					return ExchangePairTypeEnum.ETH_BTC;
				case "eth" when symbol2.ToLower() == "eur":
					return ExchangePairTypeEnum.ETH_EUR;
				case "btc" when symbol2.ToLower() == "eur":
					return ExchangePairTypeEnum.BTC_EUR;
				default:
					Logger.ErrorFormat($"No pair found for the symbols {symbol1.ToLower()}_{symbol2.ToLower()}");
					throw new ArgumentOutOfRangeException($"No pair found for the symbols {symbol1.ToLower()}_{symbol2.ToLower()}");
			}
		}

		#endregion
	}
}

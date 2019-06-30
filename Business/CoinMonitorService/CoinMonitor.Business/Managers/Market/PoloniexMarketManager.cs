using System;
using System.Collections.Generic;
using System.Linq;
using CoinMonitor.Business.Managers.Base;
using CoinMonitor.Business.Managers.EcoIndex;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.Domain.Dtos.Cex;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Dtos.Poloniex;
using CoinMonitor.Domain.Enums;
using CoinMonitor.Domain.Messages.Base;
using CoinMonitor.Domain.Messages.CexApi;
using CoinMonitor.Interfaces.DatabaseManagers;
using CoinMonitor.RestClients.Clients;
using CoinMonitor.RestClients.DatabaseClient;
using CoinMonitor.RestClients.Holders;
using Newtonsoft.Json;

namespace CoinMonitor.Business.Managers.Market
{
	public class PoloniexMarketManager: MarketWatchManager, IDataFormatter
	{
		private readonly PoloniexApiClient _poloniexApiClient;
		private readonly ElasticDatabaseClient _elasticClient;
		private readonly CoinMonitoringApiClient _coinMonitoringApiClient;
		private readonly RSIEcoIndexManager _rsiManager;
		private readonly EMAEcoIndexManager _emaManager;
		private readonly ForceIndexEcoIndexManager _forceIndexEcoIndexManager;

		public PoloniexMarketManager()
		{
			_poloniexApiClient = new PoloniexApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.PoloniexApiUrl, ""));
			_elasticClient = new ElasticDatabaseClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.ElasticApiUrl, ""), ElasticApiUrlHolder.ExchangeName.POLONIEX);
			_coinMonitoringApiClient = new CoinMonitoringApiClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.CoinMonitoringApi, ""));
			_rsiManager = new RSIEcoIndexManager(ElasticApiUrlHolder.ExchangeName.POLONIEX);
			_emaManager = new EMAEcoIndexManager(ElasticApiUrlHolder.ExchangeName.POLONIEX);
			_forceIndexEcoIndexManager = new ForceIndexEcoIndexManager(ElasticApiUrlHolder.ExchangeName.POLONIEX);
		}
		
		public override void GetMarketData()
		{
			Dictionary<string, PoloniexCurrencyDto> currencies = _poloniexApiClient.GetPoloniexCurrencies();

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

			List<PoloniexCurrencyDto> supportedCurrencyDtos = new List<PoloniexCurrencyDto>();
			foreach (var currency in currencies)
			{
				foreach (var pair in coinPair)
				{
					if (currency.Key.ToLower() == GetApiPairName(pair.symbol1, pair.symbol2) && supportedCurrencyDtos.FirstOrDefault(c => c.Name == GetApiPairName(pair.symbol1, pair.symbol2)) == null)
					{
						currency.Value.Name = currency.Key.ToLower();
						supportedCurrencyDtos.Add(currency.Value);
					}
					else if (currency.Key == "BTC_ETH" && supportedCurrencyDtos.FirstOrDefault(c => c.Name == "eth_btc") == null)
					{
						string[] splitName = currency.Key.Split('_');
						currency.Value.Name = splitName[1].ToLower() + "_" + splitName[0].ToLower();

						supportedCurrencyDtos.Add(currency.Value);
					}
				}
			}

			foreach (var pair in coinPair)
			{
				PoloniexCurrencyDto supportedCurrencyDto = supportedCurrencyDtos.FirstOrDefault(t => t.Name == GetApiPairName(pair.symbol1, pair.symbol2));
				if (supportedCurrencyDto != null)
				{
					_elasticClient.SaveRawTickerData(GetApiPairName(pair.symbol1, pair.symbol2), supportedCurrencyDto, typeof(PoloniexCurrencyDto), DateTime.Now);
					_elasticClient.SaveSanitizedDate(SanitizeTickerData(supportedCurrencyDto, pair));
				}
			}

			_rsiManager.CalculateValue();
			_emaManager.CalculateValue();
			_forceIndexEcoIndexManager.CalculateValue();
			_coinMonitoringApiClient.InvokeRecalculation(new BaseRestRequest());
		}
		
		public override void GetPairHistory()
		{
			Logger.DebugFormat("Trying to get pair history from Poloniex API");
		}

		public string GetApiPairName(string symbol1, string symbol2)
		{
			return symbol1.ToLower() + "_" + symbol2.ToLower();
		}

		public TickerFormattedDto SanitizeTickerData(BaseRestResponse response, CoinPairDto pair)
		{
			PoloniexCurrencyDto ticker = (PoloniexCurrencyDto) response;

			if (ticker != null)
			{
				return new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Poloniex,
					PairType = ResolvePairType(pair.symbol1, pair.symbol2),
					High = ticker.highestBid,
					Last = ticker.last,
					Low = ticker.lowestAsk,
					Time = DateTime.Now,
					Volume = ticker.baseVolume
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
	}
}

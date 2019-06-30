using System;
using System.Collections.Generic;
using System.Linq;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Enums;
using CoinMonitor.Domain.Messages.Elastic;
using CoinMonitor.Interfaces.EconomyManagers;
using CoinMonitor.RestClients.DatabaseClient;
using CoinMonitor.RestClients.Holders;

namespace CoinMonitor.Business.Managers.EcoIndex
{
	public class RSIEcoIndexManager: IEcoManager
	{
		private readonly ElasticApiUrlHolder.ExchangeName _exchangeName;
		private readonly ElasticDatabaseClient _elasticClient;

		public RSIEcoIndexManager(ElasticApiUrlHolder.ExchangeName exchangeName = ElasticApiUrlHolder.ExchangeName.Test)
		{
			if (exchangeName != ElasticApiUrlHolder.ExchangeName.Test)
			{
				_exchangeName = exchangeName;
				_elasticClient = new ElasticDatabaseClient(ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.ElasticApiUrl, ""), _exchangeName);
			}
		}

		public void CalculateValue()
		{
			List<ExchangePairTypeEnum> exchangePairTypeEnums = new List<ExchangePairTypeEnum>
			{
				ExchangePairTypeEnum.ETH_BTC,
				ExchangePairTypeEnum.BTC_EUR,
				ExchangePairTypeEnum.BTC_USD,
				ExchangePairTypeEnum.ETH_EUR,
				ExchangePairTypeEnum.ETH_USD
			};

			foreach (ExchangePairTypeEnum exchangePairTypeEnum in exchangePairTypeEnums)
			{
				List<TickerFormattedDto> tickers = LoadTickerFormattedDtos(exchangePairTypeEnum);

				if (tickers.Count > 0)
				{
					decimal RSI = CalculateRSI(tickers);
					SaveRSIValue(RSI, exchangePairTypeEnum);
				}
			}
		}

		public decimal CalculateRSI(List<TickerFormattedDto> tickers)
		{
			List<decimal> loses = new List<decimal>();
			List<decimal> gains = new List<decimal>();

			if (tickers.Count > 1)
			{
				for (int i = 1; i < tickers.Count; i++)
				{
					if (tickers[i].Last < 0)
					{
						return 0M;
					}

					decimal value = tickers[i - 1].Last - tickers[i].Last;

					if (value > 0)
					{
						gains.Add(value);
					}
					else
					{
						loses.Add(value);
					}
				}

				decimal averageLoss = 0M;
				if (loses.Count > 0)
				{
					averageLoss = loses.Average();
				}

				decimal averageGain = 0M;
				if (gains.Count > 0)
				{
					averageGain = gains.Average();
				}

				decimal RS = averageGain;
				if (averageLoss > 0)
				{
					RS = averageGain / averageLoss;
				}

				decimal RSI = 100 - (100 / (1 + RS));
				return RSI;
			}

			return 0M;
		}


		private List<TickerFormattedDto> LoadTickerFormattedDtos(ExchangePairTypeEnum exchangePairTypeEnum)
		{
			GetFormattedDataRequest tickerRequest = new GetFormattedDataRequest
			{
				From = DateTime.Now.AddMinutes(-25),
				To = DateTime.Now,
				PatterName = _exchangeName.ToString().ToLower() + "_formatted",
				PairType = (int)exchangePairTypeEnum,
				ExchangeType = _exchangeName == ElasticApiUrlHolder.ExchangeName.CEX ? 1 : 2,
				Size = 25
			};

			GetFormattedDataResponse response = _elasticClient.GetFormattedTickers(tickerRequest);

			List<TickerFormattedDto> tickers = response.hits.hits.Select(r => r._source).ToList();
			return tickers;
		}
		
		private void SaveRSIValue(decimal RSIValue, ExchangePairTypeEnum pairTypeEnum)
		{
			if (RSIValue == 0)
				return;

			Domain.Dtos.EcoIndex.EcoIndex ecoIndex = new Domain.Dtos.EcoIndex.EcoIndex
			{
				Id = EcoIndexEnum.RSI,
				Name = ResolvePairTypeName(pairTypeEnum),
				Time = DateTime.Now,
				Value =  RSIValue
			};

			_elasticClient.SaveEcoIndexData(ecoIndex);
		}

		private string ResolvePairTypeName(ExchangePairTypeEnum pairTypeEnum)
		{
			switch (pairTypeEnum)
			{
				case ExchangePairTypeEnum.BTC_USD:
					return "btc_usd";
				case ExchangePairTypeEnum.ETH_USD:
					return "eth_usd";
				case ExchangePairTypeEnum.ETH_BTC:
					return "eth_btc";
				case ExchangePairTypeEnum.ETH_EUR:
					return "eth_eur";
				case ExchangePairTypeEnum.BTC_EUR:
					return "btc_eur";
			}

			return "unknown";
		}
	}
}

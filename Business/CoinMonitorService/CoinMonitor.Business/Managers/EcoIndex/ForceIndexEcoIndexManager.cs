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
	public class ForceIndexEcoIndexManager: IEcoManager
	{
		private readonly ElasticApiUrlHolder.ExchangeName _exchangeName;
		private readonly ElasticDatabaseClient _elasticClient;

		public ForceIndexEcoIndexManager(ElasticApiUrlHolder.ExchangeName exchangeName = ElasticApiUrlHolder.ExchangeName.Test)
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
				List<TickerFormattedDto> allTickers = LoadTickerFormattedDtos(exchangePairTypeEnum);
				List<TickerFormattedDto> tickers = allTickers.Where(t => t.PairType == exchangePairTypeEnum).OrderByDescending(t => t.Time).Take(2).ToList();
				List<Domain.Dtos.EcoIndex.EcoIndex> LastEMAValues = GetLastEmaValues(exchangePairTypeEnum);

				decimal forceIndex = CalculateForceIndex(tickers, LastEMAValues);
				SaveForceIndexValue(forceIndex, exchangePairTypeEnum);
			}
		}

		public decimal CalculateForceIndex(List<TickerFormattedDto> tickers,List<Domain.Dtos.EcoIndex.EcoIndex> LastEMAValues)
		{
			if (LastEMAValues.Count >= 2)
			{
				decimal EmaSum = LastEMAValues.Sum(l => l.Value);

				if (EmaSum < 0)
				{
					return 0M;
				}

				if (tickers.Count >= 2)
				{
					decimal forceIndex = EmaSum * (tickers[0].Volume * (tickers[0].Last - tickers[1].Last));
					return forceIndex;
				}
			}

			return 0M;
		}

		private void SaveForceIndexValue(decimal ForceIndexValue, ExchangePairTypeEnum pairTypeEnum)
		{
			if (ForceIndexValue == 0)
				return;

			Domain.Dtos.EcoIndex.EcoIndex ecoIndex = new Domain.Dtos.EcoIndex.EcoIndex
			{
				Id = EcoIndexEnum.ForceIndex,
				Name = ResolvePairTypeName(pairTypeEnum),
				Time = DateTime.Now,
				Value = ForceIndexValue
			};

			_elasticClient.SaveEcoIndexData(ecoIndex);
		}

		private List<Domain.Dtos.EcoIndex.EcoIndex> GetLastEmaValues(ExchangePairTypeEnum exchangePairTypeEnum)
		{
			List<Domain.Dtos.EcoIndex.EcoIndex> ecoIndexes = _elasticClient.GetLastEcoIndex(_exchangeName.ToString().ToLower() + "_eco_index", EcoIndexEnum.EMA, ResolvePairTypeName(exchangePairTypeEnum),2);

			return ecoIndexes;
		}

		private List<TickerFormattedDto> LoadTickerFormattedDtos(ExchangePairTypeEnum exchangePairTypeEnum)
		{
			GetFormattedDataRequest tickerRequest = new GetFormattedDataRequest
			{
				From = DateTime.Now.AddMinutes(-25),
				To = DateTime.Now,
				PatterName = _exchangeName.ToString().ToLower() + "_formatted",
				ExchangeType = (int)exchangePairTypeEnum,
				PairType = _exchangeName == ElasticApiUrlHolder.ExchangeName.CEX ? 1 : 2,
				Size = 25
			};

			GetFormattedDataResponse response = _elasticClient.GetFormattedTickers(tickerRequest);

			List<TickerFormattedDto> tickers = response.hits.hits.Select(r => r._source).ToList();
			return tickers;
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

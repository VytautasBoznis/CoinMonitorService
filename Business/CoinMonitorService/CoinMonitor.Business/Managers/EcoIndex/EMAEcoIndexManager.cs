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
	public class EMAEcoIndexManager: IEcoManager
	{
		private readonly ElasticApiUrlHolder.ExchangeName _exchangeName;
		private readonly ElasticDatabaseClient _elasticClient;

		public EMAEcoIndexManager(ElasticApiUrlHolder.ExchangeName exchangeName = ElasticApiUrlHolder.ExchangeName.Test)
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
				Domain.Dtos.EcoIndex.EcoIndex lastEMA = GetLastEmaValue(exchangePairTypeEnum);

				decimal EMA = CalculateEma(tickers, lastEMA);
				SaveEMAValue(EMA, exchangePairTypeEnum);
			}
		}

		public decimal CalculateEma(List<TickerFormattedDto> tickers, Domain.Dtos.EcoIndex.EcoIndex lastEMA)
		{
			if (tickers.Count > 0)
			{
				decimal SMA = tickers.Average(t => t.Last);
				decimal multiplier = 2M / (tickers.Count + 1M);

				TickerFormattedDto lastTickerFormattedDto = tickers?.LastOrDefault();

				if (lastTickerFormattedDto == null)
				{
					return 0M;
				}

				decimal lastEMAValue = 0M;

				if (lastEMA != null)
				{
					lastEMAValue = lastEMA.Value;
				}

				decimal EMA = (lastTickerFormattedDto.Last - lastEMAValue) * multiplier + lastEMAValue;

				if (EMA < 0)
				{
					return 0M;
				}

				return EMA;
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

			List<TickerFormattedDto> tickers = response?.hits?.hits?.Select(r => r._source).ToList();
			return tickers;
		}

		private void SaveEMAValue(decimal EMAValue, ExchangePairTypeEnum pairTypeEnum)
		{
			if (EMAValue == 0)
				return;

			Domain.Dtos.EcoIndex.EcoIndex ecoIndex = new Domain.Dtos.EcoIndex.EcoIndex
			{
				Id = EcoIndexEnum.EMA,
				Name = ResolvePairTypeName(pairTypeEnum),
				Time = DateTime.Now,
				Value = EMAValue
			};

			_elasticClient.SaveEcoIndexData(ecoIndex);
		}

		private Domain.Dtos.EcoIndex.EcoIndex GetLastEmaValue(ExchangePairTypeEnum exchangePairTypeEnum)
		{
			List<Domain.Dtos.EcoIndex.EcoIndex> ecoIndex = _elasticClient.GetLastEcoIndex(_exchangeName.ToString().ToLower() + "_eco_index", EcoIndexEnum.EMA, ResolvePairTypeName(exchangePairTypeEnum));

			return ecoIndex?.FirstOrDefault();
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

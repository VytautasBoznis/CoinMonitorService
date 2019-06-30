using System;
using System.Collections.Generic;
using CoinMonitor.Business.Managers.EcoIndex;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoinMonitorService.Tests
{
	[TestClass]
	public class EmaTests
	{
		[TestMethod]
		public void WithNotEnoughData()
		{
			EMAEcoIndexManager emaManager = new EMAEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
			};

			EcoIndex lastEMA = null;

			decimal Ema = emaManager.CalculateEma(tickers, lastEMA);
			Assert.AreEqual(0M, Ema);
		}

		[TestMethod]
		public void NormalCalculation()
		{
			EMAEcoIndexManager emaManager = new EMAEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 110,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			EcoIndex lastEMA = new EcoIndex
			{
				Id = EcoIndexEnum.EMA,
				Name = "BTC_ETH",
				Time = DateTime.Now,
				Value = 110
			};
			decimal Ema = emaManager.CalculateEma(tickers, lastEMA);
			Assert.AreEqual(110M, Ema);
		}

		[TestMethod]
		public void NormalCalculation2()
		{
			EMAEcoIndexManager emaManager = new EMAEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 110,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 120,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 120,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			EcoIndex lastEMA = new EcoIndex
			{
				Id = EcoIndexEnum.EMA,
				Name = "BTC_ETH",
				Time = DateTime.Now,
				Value = 110
			};
			decimal Ema = emaManager.CalculateEma(tickers, lastEMA);
			Assert.AreEqual(114M, Ema);
		}

		[TestMethod]
		public void BadData()
		{
			EMAEcoIndexManager emaManager = new EMAEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = -110,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			EcoIndex lastEMA = new EcoIndex
			{
				Id = EcoIndexEnum.EMA,
				Name = "BTC_ETH",
				Time = DateTime.Now,
				Value = -110
			};

			decimal Ema = emaManager.CalculateEma(tickers, lastEMA);
			Assert.AreEqual(0M, Ema);
		}

		[TestMethod]
		public void ZeroReturn()
		{
			EMAEcoIndexManager emaManager = new EMAEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = -100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			EcoIndex lastEMA = new EcoIndex
			{
				Id = EcoIndexEnum.EMA,
				Name = "BTC_ETH",
				Time = DateTime.Now,
				Value = -110
			};

			decimal Ema = emaManager.CalculateEma(tickers, lastEMA);
			Assert.AreEqual(0M, Ema);
		}
	}
}

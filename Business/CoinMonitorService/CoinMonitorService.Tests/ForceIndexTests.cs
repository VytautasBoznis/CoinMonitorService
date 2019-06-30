using System;
using System.Collections.Generic;
using CoinMonitor.Business.Managers.EcoIndex;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoinMonitorService.Tests
{
	[TestClass]
	public class ForceIndexTests
	{
		[TestMethod]
		public void WithNotEnoughData()
		{
			ForceIndexEcoIndexManager manager = new ForceIndexEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = -111,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			List<EcoIndex> indexes = new List<EcoIndex>
			{
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = 110
				},
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = 100
				},
			};

			decimal force = manager.CalculateForceIndex(tickers, indexes);
			Assert.AreEqual(0M, force);
		}

		[TestMethod]
		public void NormalCalculation()
		{
			ForceIndexEcoIndexManager manager = new ForceIndexEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 111,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			List<EcoIndex> indexes = new List<EcoIndex>
			{
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = 110
				},
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = 100
				},
			};

			decimal force = manager.CalculateForceIndex(tickers, indexes);
			Assert.AreEqual(231000M, force);
		}

		[TestMethod]
		public void NormalCalculation2()
		{
			ForceIndexEcoIndexManager manager = new ForceIndexEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 111,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			List<EcoIndex> indexes = new List<EcoIndex>
			{
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = 110
				},
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = 100
				},
			};

			decimal force = manager.CalculateForceIndex(tickers, indexes);
			Assert.AreEqual(231000M, force);
		}

		[TestMethod]
		public void BadData()
		{
			ForceIndexEcoIndexManager manager = new ForceIndexEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 111,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			List<EcoIndex> indexes = new List<EcoIndex>
			{
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = -110
				},
				new EcoIndex
				{
					Id = EcoIndexEnum.EMA,
					Name = "BTC_ETH",
					Time = DateTime.Now,
					Value = -100
				},
			};

			decimal force = manager.CalculateForceIndex(tickers, indexes);
			Assert.AreEqual(0M, force);
		}

		[TestMethod]
		public void ZeroReturn()
		{
			ForceIndexEcoIndexManager manager = new ForceIndexEcoIndexManager();

			List<TickerFormattedDto> tickers = new List<TickerFormattedDto>
			{
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 111,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				},new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 100,
					Last = 100,
					Low = 100,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 100
				}
			};

			List<EcoIndex> indexes = new List<EcoIndex>
			{
				
			};

			decimal force = manager.CalculateForceIndex(tickers, indexes);
			Assert.AreEqual(0M, force);
		}
	}
}

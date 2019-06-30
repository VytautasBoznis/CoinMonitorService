using System;
using System.Collections.Generic;
using CoinMonitor.Business.Managers.EcoIndex;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Enums;
using CoinMonitor.RestClients.Holders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoinMonitorService.Tests
{
	[TestClass]
	public class RSITests
	{
		[TestMethod]
		public void WithNotEnoughData()
		{
			RSIEcoIndexManager rsiManager = new RSIEcoIndexManager();

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
				}
			};

			decimal rsi = rsiManager.CalculateRSI(tickers);
			Assert.AreEqual(0M, rsi);
		}

		[TestMethod]
		public void NormalCalculation()
		{
			RSIEcoIndexManager rsiManager = new RSIEcoIndexManager();

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
					High = 120,
					Last = 120,
					Low = 120,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 120
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 110,
					Last = 110,
					Low = 110,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 110
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 130,
					Last = 130,
					Low = 130,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 130
				},
			};

			decimal rsi = rsiManager.CalculateRSI(tickers);
			Assert.AreNotEqual(0M, rsi);
		}

		[TestMethod]
		public void NormalCalculation2()
		{
			RSIEcoIndexManager rsiManager = new RSIEcoIndexManager();

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
					High = 120,
					Last = 120,
					Low = 120,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 120
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 110,
					Last = 110,
					Low = 110,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 110
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 130,
					Last = 130,
					Low = 130,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 130
				},
			};

			decimal rsi = rsiManager.CalculateRSI(tickers);
			Assert.AreNotEqual(0M, rsi);
		}

		[TestMethod]
		public void BadData()
		{
			RSIEcoIndexManager rsiManager = new RSIEcoIndexManager();

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
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 120,
					Last = -111,
					Low = 120,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 120
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 110,
					Last = -111,
					Low = -111,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 110
				},
				new TickerFormattedDto
				{
					ExchangeType = ExchangeTypeEnum.Cex,
					High = 130,
					Last = 130,
					Low = 130,
					PairType = ExchangePairTypeEnum.ETH_BTC,
					Time = DateTime.Now,
					Volume = 130
				},
			};

			decimal rsi = rsiManager.CalculateRSI(tickers);
			Assert.AreEqual(0M, rsi);
		}

		[TestMethod]
		public void ZeroReturn()
		{
			RSIEcoIndexManager rsiManager = new RSIEcoIndexManager();

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

			decimal rsi = rsiManager.CalculateRSI(tickers);
			Assert.AreEqual(0M, rsi);
		}
	}
}

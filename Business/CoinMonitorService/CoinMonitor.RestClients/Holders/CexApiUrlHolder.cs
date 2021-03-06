﻿namespace CoinMonitor.RestClients.Holders
{
	public static class CexApiUrlHolder
	{
		public const string PriceStatsUrl = "/price_stats/{symbol1}/{symbol2}";
		public const string CurrencyLimitsUrl = "currency_limits";
		public const string TickerUrl = "/tickers/{symbol1}/{symbol2}/{symbol3}/{symbol4}";
	}
}

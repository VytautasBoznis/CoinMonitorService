﻿using CoinMonitor.Domain.Messages.CexApi;

namespace CoinMonitor.Interfaces.OutsideApiManagers
{
	public interface ICexApiManager
	{
		PriceStatsResponse PriseStats(string symbol1, string symbol2, PriceStatsRequest request);
	}
}

using System.Collections.Generic;
using CoinMonitor.Domain.Messages.CexApi;

namespace CoinMonitor.Interfaces.OutsideApiManagers
{
	public interface ICexApiManager
	{
		List<TickerResponse> TickPairs(string symbol1, string symbol2, string symbol3, string symbol4);
	}
}

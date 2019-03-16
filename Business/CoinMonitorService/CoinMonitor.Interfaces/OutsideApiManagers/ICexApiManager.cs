using CoinMonitor.Domain.Messages.CexApi;

namespace CoinMonitor.Interfaces.OutsideApiManagers
{
	public interface ICexApiManager
	{
		TickerResponse TickPair(string symbol1, string symbol2);
	}
}

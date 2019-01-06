using System.Collections.Generic;
using CoinMonitor.Domain.Dtos.Poloniex;

namespace CoinMonitor.Interfaces.OutsideApiManagers
{
	public interface IPoloniexApiManager
	{
		Dictionary<string, PoloniexCurrencyDto> GetPoloniexCurrencies();
	}
}

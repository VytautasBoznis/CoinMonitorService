using CoinMonitor.Domain.Dtos.Cex;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Enums;
using CoinMonitor.Domain.Messages.Base;

namespace CoinMonitor.Interfaces.DatabaseManagers
{
	public interface IDataFormatter
	{
		string GetApiPairName(string symbol1, string symbol2);
		ExchangePairTypeEnum ResolvePairType(string symbol1, string symbol2);
		TickerFormattedDto SanitizeTickerData(BaseRestResponse response, CoinPairDto pair);
	}
}

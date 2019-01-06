using System.Collections.Generic;
using CoinMonitor.Domain.Dtos.Cex;

namespace CoinMonitor.Domain.Messages.CexApi
{
	public class CurrencyLimitsRequest : BaseCexRequest
	{
	}

	public class CurrencyLimitsResponse : BaseCexResponse<Dictionary<string, List<CurrencyLimitDto>>>
	{
	}
}

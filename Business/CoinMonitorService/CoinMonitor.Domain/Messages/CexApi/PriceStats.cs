using CoinMonitor.Domain.Messages.Base;

namespace CoinMonitor.Domain.Messages.CexApi
{
	public class PriceStatsRequest : BaseRequest
	{
		public int lastHours { get; set; }
		public int maxRespArrSize { get; set; }
	}

	public class PriceStatsResponse : BaseResponse
	{
		public string tmsp { get; set; }
		public string prce { get; set; }
	}
}

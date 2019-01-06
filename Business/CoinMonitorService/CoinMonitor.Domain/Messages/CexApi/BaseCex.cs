using CoinMonitor.Domain.Messages.Base;

namespace CoinMonitor.Domain.Messages.CexApi
{
	public class BaseCexRequest: BaseRestRequest
	{
	}

	public class BaseCexResponse<T> : BaseRestResponse
	{
		public string E { get; set; }
		public string Ok { get; set; }
		public T Data { get; set; }
	}
}

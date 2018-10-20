using CoinMonitor.Interfaces.OutsideApiManagers;
using CoinMonitor.RestClients.BaseClients;

namespace CoinMonitor.RestClients.Clients
{
	public class PoloniexApiClient: BaseRestClient, IPoloniexApiManager
	{
		public PoloniexApiClient(string apiUrl) : base(apiUrl)
		{
		}
	}
}

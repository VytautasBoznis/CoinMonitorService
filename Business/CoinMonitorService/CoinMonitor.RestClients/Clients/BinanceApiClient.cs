using CoinMonitor.Interfaces.OutsideApiManagers;
using CoinMonitor.RestClients.BaseClients;

namespace CoinMonitor.RestClients.Clients
{
	public class BinanceApiClient: BaseRestClient, IBinanceApiManager 
	{
		public BinanceApiClient(string apiUrl) : base(apiUrl)
		{
		}
	}
}

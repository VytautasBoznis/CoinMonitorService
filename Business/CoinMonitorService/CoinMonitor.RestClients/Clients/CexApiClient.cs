using System.Collections.Generic;
using CoinMonitor.Domain.Messages.CexApi;
using CoinMonitor.Interfaces.OutsideApiManagers;
using CoinMonitor.RestClients.BaseClients;
using CoinMonitor.RestClients.Holders;
using RestSharp;

namespace CoinMonitor.RestClients.Clients
{
	public class CexApiClient: BaseRestClient, ICexApiManager
	{
		public CexApiClient(string apiUrl) : base(apiUrl)
		{}

		public List<TickerResponse> TickPairs(string symbol1, string symbol2, string symbol3, string symbol4)
		{
			Dictionary<string, string> urlParams = new Dictionary<string, string> { {"symbol1", symbol1}, { "symbol2", symbol2 }, { "symbol3", symbol3 }, { "symbol4", symbol4 } };
			var restRequest = HandleRequest(CexApiUrlHolder.TickerUrl, new TickerRequest(), Method.GET, urlParams);
			var response = HandleGenericResponse(Client.Execute<BaseCexResponse<List<TickerResponse>>>(restRequest));
			return response.Data.Data;
		}
	}
}

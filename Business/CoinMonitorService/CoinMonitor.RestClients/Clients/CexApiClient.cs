using System.Collections.Generic;
using CoinMonitor.Base.Operations;
using CoinMonitor.Domain.Messages.CexApi;
using CoinMonitor.Interfaces.OutsideApiManagers;
using CoinMonitor.RestClients.BaseClients;
using CoinMonitor.RestClients.Holders;
using RestSharp;

namespace CoinMonitor.RestClients.Clients
{
	public class CexApiClient: BaseRestClient, ICexApiManager
	{
		public CexApiClient() : base("https://cex.io/api")
		{
		}

		public PriceStatsResponse PriseStats(string symbol1, string symbol2, PriceStatsRequest request)
		{
			using (new OperationTimer(this.GetType(), System.Reflection.MethodBase.GetCurrentMethod().Name))
			{
				var urlParameters = new Dictionary<string, string> {{"symbol1", symbol1}, {"symbol2", symbol2}};
				var restRequest = HandleRequest(CexApiUrlHolder.PriceStatsUrl, request, Method.POST, urlParameters);
				var response = HandleResponse(Client.Execute<PriceStatsResponse>(restRequest));
				return response.Data;
			}
		}
	}
}

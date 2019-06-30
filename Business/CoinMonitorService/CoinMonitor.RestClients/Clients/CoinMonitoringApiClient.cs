using CoinMonitor.Domain.Messages.Base;
using CoinMonitor.Interfaces.OutsideApiManagers;
using CoinMonitor.RestClients.BaseClients;
using CoinMonitor.RestClients.Holders;
using RestSharp;

namespace CoinMonitor.RestClients.Clients
{
	public class CoinMonitoringApiClient : BaseRestClient, ICoinMonitoringApiManager
	{
		public CoinMonitoringApiClient(string apiUrl) : base(apiUrl)
		{
		}

		public BaseRestResponse InvokeRecalculation(BaseRestRequest request)
		{
			var restRequest = HandleRequest(CoinMonitoringApiUrlHolder.RecalculateActions, request, Method.POST);
			var response = HandleGenericResponse(Client.Execute<BaseRestResponse>(restRequest));
			return response.Data;
		}
	}
}

using System.Collections.Generic;
using CoinMonitor.Core.Timers;
using CoinMonitor.Domain.Dtos.Poloniex;
using CoinMonitor.Domain.Messages.Base;
using CoinMonitor.Domain.Messages.CexApi;
using CoinMonitor.Interfaces.OutsideApiManagers;
using CoinMonitor.RestClients.BaseClients;
using CoinMonitor.RestClients.Holders;
using RestSharp;

namespace CoinMonitor.RestClients.Clients
{
	public class PoloniexApiClient: BaseRestClient, IPoloniexApiManager
	{
		public PoloniexApiClient(string apiUrl) : base(apiUrl)
		{
		}

		public Dictionary<string, PoloniexCurrencyDto> GetPoloniexCurrencies()
		{
			using (new OperationTimer(this.GetType(), System.Reflection.MethodBase.GetCurrentMethod().Name))
			{
				var restRequest = HandleRequest(PoloniexApiUrlHolder.ReturnCurrencies, new BaseRestRequest(), Method.GET);
				var response = HandleGenericResponse(Client.Execute<Dictionary<string, PoloniexCurrencyDto>>(restRequest));
				return response.Data;
			}
		}
	}
}

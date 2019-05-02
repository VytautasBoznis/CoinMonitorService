using System;
using System.Net;
using CoinMonitor.Domain.Dtos.Cex;
using CoinMonitor.Interfaces.DatabaseManagers;
using CoinMonitor.RestClients.Holders;
using log4net;
using RestSharp;

namespace CoinMonitor.RestClients.DatabaseClient
{
	public class ElasticDatabaseClient: IElasticDatabaseClient
	{
		private readonly ElasticApiUrlHolder.ExchangeName _exchangeName;
		private readonly IRestClient _client;
		private readonly ILog _Logger;

		public ElasticDatabaseClient(string apiUrl, ElasticApiUrlHolder.ExchangeName excName)
		{
			_Logger = LogManager.GetLogger("ElasticDatabaseLogger");
			_Logger.DebugFormat($"DB Logger online for [{excName}]");
			_client = new RestClient(apiUrl);
			_exchangeName = excName;
		}


		public void SaveRawTickerData(string pair, object tickerResponse, Type responseType)
		{
			RestRequest request = PrepareSaveRequest(Method.POST, ElasticApiUrlHolder.DataType.RAW, pair, tickerResponse, responseType);
			HandleElasticResponse(_client.Execute(request));
		}

		#region BaseHandling

		protected RestRequest PrepareSaveRequest(Method method, ElasticApiUrlHolder.DataType dataType, string pairName, object requestData, Type requestType)
		{
			string requestUrl = ElasticApiUrlHolder.GenerateDataSaveUrl(_exchangeName, dataType, pairName);
			RestRequest request = new RestRequest(requestUrl, method);
			request.RequestFormat = DataFormat.Json;
			request.AddBody(Convert.ChangeType(requestData, requestType));

			return request;
		}

		protected IRestResponse HandleElasticResponse(IRestResponse response)
		{
			if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
			{
				_Logger.DebugFormat($"Elastic save completed for {_exchangeName.ToString()} Method: {response.Request.Method}");
			}
			else
			{
				_Logger.ErrorFormat($"Error happened trying to do action [{_exchangeName.ToString()}] data error: [{response.Content}]");
			}

			return response;
		}

		#endregion

	}
}

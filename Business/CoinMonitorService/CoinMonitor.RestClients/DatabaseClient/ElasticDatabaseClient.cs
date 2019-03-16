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


		public void SaveRawTickerData(string pair, object tickerResponse)
		{
			RestRequest request = PrepareSaveRequest(Method.PUT, ElasticApiUrlHolder.DataType.RAW, pair,tickerResponse);
			HandleElasticResponse(_client.Execute(request));
		}

		#region BaseHandling

		protected RestRequest PrepareSaveRequest(Method method, ElasticApiUrlHolder.DataType dataType, string pairName, object requestData)
		{
			string requestUrl = ElasticApiUrlHolder.GenerateDataPutUrl(_exchangeName, dataType, pairName);
			RestRequest request = new RestRequest(requestUrl, method);
			request.AddBody(requestData);

			return request;
		}

		protected IRestResponse HandleElasticResponse(IRestResponse response)
		{
			if (response.ErrorException != null)
			{
				_Logger.ErrorFormat($"Error happened trying to save [{_exchangeName.ToString()}] data error: [{response.ErrorException.Message}]");
			}
			else
			{
				_Logger.DebugFormat($"Elastic save completed for {_exchangeName.ToString()} Method: {response.Request.Method}");
			}

			return response;
		}

		#endregion

	}
}

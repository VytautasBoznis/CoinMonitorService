using System.Collections.Generic;
using CoinMonitor.Domain.Messages.Base;
using log4net;
using RestSharp;

namespace CoinMonitor.RestClients.BaseClients
{
	public abstract class BaseRestClient
	{
		public RestClient Client;
		private readonly ILog _logger;

		protected BaseRestClient(string apiUrl)
		{
			_logger = LogManager.GetLogger(apiUrl);
			_logger.DebugFormat($"Starting {apiUrl} base service client");
			Client = new RestClient(apiUrl);
		}

		public RestRequest HandleRequest(string requestUrl, BaseRestRequest request, Method method, Dictionary<string, string> urlParameterDictionary = null)
		{
			RestRequest restRequest = new RestRequest(requestUrl, method);
			restRequest.AddBody(request);

			if (urlParameterDictionary != null)
			{
				foreach (var urlParameters in urlParameterDictionary)
				{
					restRequest.AddUrlSegment(urlParameters.Key, urlParameters.Value);
				}
			}

			_logger.DebugFormat($"Request for {requestUrl} created");
			return restRequest;
		}
		
		public IRestResponse<T> HandleResponse<T>(IRestResponse<T> response) where T: BaseRestResponse
		{
			_logger.DebugFormat($"Rest response url {response.ResponseUri} body content {response.Content}");
			return response;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CoinMonitor.Domain.Dtos.Cex;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Dtos.EcoIndex.Formatting;
using CoinMonitor.Domain.Dtos.Elastic;
using CoinMonitor.Domain.Enums;
using CoinMonitor.Domain.Messages.Elastic;
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

		public void SaveRawTickerData(string pair, object tickerResponse, Type responseType, DateTime time)
		{
			RestRequest request = PrepareSaveRequest(Method.POST, ElasticApiUrlHolder.DataType.RAW, pair, tickerResponse, responseType, time);
			HandleElasticResponse(_client.Execute(request));
		}

		public void SaveSanitizedDate(TickerFormattedDto formattedDto)
		{
			RestRequest request = PrepareSaveRequest(Method.POST, ElasticApiUrlHolder.DataType.FORMATTED, formattedDto.PairType.ToString().ToLower(), formattedDto, typeof(TickerFormattedDto), formattedDto.Time);
			HandleElasticResponse(_client.Execute(request));
		}

		public GetFormattedDataResponse GetFormattedTickers(GetFormattedDataRequest request)
		{
			RestRequest restRequest = new RestRequest(request.PatterName +"/_search", Method.POST);

			ElasticMultiRequest searchRequest = new ElasticMultiRequest
			{
				query = new ElasticFormattedQuery
				{
					@bool = new ElasticFormattedBool
					{
						filter = new ElasticFormattedFilter
						{
							term = new ElasticTermExchangeNr
							{
								ExchangeType = request.ExchangeType
							}
						},
						must = new ElasticFormattedMustFilter
						{
							term = new ElasticMustPair
							{
								PairType = request.PairType
							}
						},
						should = new ElasticFormattedShouldFilter
						{
							range = new ElasticRange
							{
								Time = new ElasticRangeParameter
								{
									gte = request.From.ToString("yyyy-MM-dd"),
									lte = request.To.ToString("yyyy-MM-dd")
								}
							}
						}
					}
				},
				size = request.Size,
				sort = new List<ElasticSort>
				{
					new ElasticSort
					{
						Time = new ElasticTimeSort()
					}
				}
			};

			restRequest.AddJsonBody(searchRequest);
			IRestResponse<GetFormattedDataResponse> response = _client.Execute<GetFormattedDataResponse>(restRequest);
			return response.Data;
		}

		public void SaveEcoIndexData(EcoIndex ecoIndex)
		{
			RestRequest request = PrepareEcoIndexSaveRequest(Method.POST, ElasticApiUrlHolder.DataType.ECO_INDEX, ecoIndex, ecoIndex.Time);
			HandleElasticResponse(_client.Execute(request));
		}

		public List<EcoIndex> GetLastEcoIndex(string pattern, EcoIndexEnum ecoIndex, string pairName, int size =1)
		{
			RestRequest restRequest = new RestRequest(pattern + "/_search", Method.POST);
			ElasticSingleRequest request = new ElasticSingleRequest
			{ 
				query = new ElasticEcoQuery
				{
					@bool = new ElasticBool
					{
						filter = new ElasticFilter
						{
							term = new ElasticTermId
							{
								Id = (int) ecoIndex
							}
						},
						must = new ElasticMustFilter
						{
							term = new ElasticMustName
							{
								Name = pairName
							}
						},
						
					}
				},
				size = size,
				sort = new List<ElasticSort>
				{
					new ElasticSort
					{
						Time = new ElasticTimeSort()
					}
				}
			};

			restRequest.AddJsonBody(request);
			IRestResponse<ElasticItemResponse> response = _client.Execute<ElasticItemResponse>(restRequest);
			return response?.Data?.hits?.hits?.Select(h => h._source)?.ToList();
		}

		#region BaseHandling

		protected RestRequest PrepareSaveRequest(Method method, ElasticApiUrlHolder.DataType dataType, string pairName, object requestData, Type requestType, DateTime time)
		{
			string requestUrl = ElasticApiUrlHolder.GenerateDataSaveUrl(_exchangeName, dataType, pairName, time);
			RestRequest request = new RestRequest(requestUrl, method);
			request.RequestFormat = DataFormat.Json;
			request.AddBody(Convert.ChangeType(requestData, requestType));

			return request;
		}

		protected RestRequest PrepareEcoIndexSaveRequest(Method method, ElasticApiUrlHolder.DataType dataType, EcoIndex index, DateTime time)
		{
			string requestUrl = ElasticApiUrlHolder.GenerateDataSaveUrl(_exchangeName, dataType, ResolveEcoIndexName(index.Id).ToLower(), time);
			RestRequest request = new RestRequest(requestUrl, method);
			request.RequestFormat = DataFormat.Json;
			request.AddBody(index);

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

		protected string ResolveEcoIndexName(EcoIndexEnum ecoIndex)
		{
			switch (ecoIndex)
			{
				case EcoIndexEnum.RSI:
					return "rsi";
				case EcoIndexEnum.EMA:
					return "ema";
				case EcoIndexEnum.ForceIndex:
					return "force_index";
			}

			return "unknown";
		}

		#endregion

	}
}

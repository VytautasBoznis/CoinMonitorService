using System;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Dtos.Elastic;
using CoinMonitor.Domain.Messages.Base;

namespace CoinMonitor.Domain.Messages.Elastic
{
	public class GetFormattedDataRequest: BaseRestRequest
	{
		public string PatterName { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public int ExchangeType { get; set; }
		public int PairType { get; set; }
		public int Size { get; set; }
	}
	public class GetFormattedDataResponse : BaseRestRequest
	{
		public int took { get; set; }
		public ElasticHitWrapper<TickerFormattedDto> hits { get; set; }
	}
}

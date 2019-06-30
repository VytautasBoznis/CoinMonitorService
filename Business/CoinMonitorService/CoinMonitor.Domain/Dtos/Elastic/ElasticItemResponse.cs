using System.Collections.Generic;

namespace CoinMonitor.Domain.Dtos.Elastic
{
	public class ElasticItemResponse
	{
		public int took { get; set; }
		public ElasticHitWrapper<EcoIndex.EcoIndex> hits { get; set; }
	}
}

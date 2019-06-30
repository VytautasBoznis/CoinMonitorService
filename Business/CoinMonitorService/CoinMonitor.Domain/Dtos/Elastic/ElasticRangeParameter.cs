using System;

namespace CoinMonitor.Domain.Dtos.Elastic
{
	public class ElasticRangeParameter
	{
		public string gte { get; set; }
		public string lte { get; set; }
	}
}

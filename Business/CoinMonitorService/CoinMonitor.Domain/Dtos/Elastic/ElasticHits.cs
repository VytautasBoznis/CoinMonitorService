using System.Collections.Generic;

namespace CoinMonitor.Domain.Dtos.Elastic
{
	public class ElasticHits<T> 
	{
		public List<T> hits { get; set; }
	}
}

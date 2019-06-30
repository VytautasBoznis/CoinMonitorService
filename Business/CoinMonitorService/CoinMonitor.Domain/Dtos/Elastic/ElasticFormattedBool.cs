using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitor.Domain.Dtos.Elastic
{
	public class ElasticFormattedBool
	{
		public ElasticFormattedFilter filter { get; set; }
		public ElasticFormattedMustFilter must { get; set; }
		public ElasticFormattedShouldFilter should { get; set; }
	}
}

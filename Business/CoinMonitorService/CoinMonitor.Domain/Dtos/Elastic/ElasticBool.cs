namespace CoinMonitor.Domain.Dtos.Elastic
{
	public class ElasticBool
	{
		public ElasticFilter filter { get; set; }
		public ElasticMustFilter must { get; set; }
	}
}

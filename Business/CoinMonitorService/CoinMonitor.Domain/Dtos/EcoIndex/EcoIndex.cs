using System;
using CoinMonitor.Domain.Enums;

namespace CoinMonitor.Domain.Dtos.EcoIndex
{
	public class EcoIndex
	{
		public EcoIndexEnum Id { get; set; }
		public string Name { get; set; }
		public DateTime Time { get; set; }
		public decimal Value { get; set; }
	}
}

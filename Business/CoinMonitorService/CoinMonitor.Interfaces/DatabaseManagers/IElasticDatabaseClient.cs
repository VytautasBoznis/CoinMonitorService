using System;
using System.Collections.Generic;
using CoinMonitor.Domain.Dtos.EcoIndex;
using CoinMonitor.Domain.Dtos.EcoIndex.Formatting;
using CoinMonitor.Domain.Enums;
using CoinMonitor.Domain.Messages.Elastic;

namespace CoinMonitor.Interfaces.DatabaseManagers
{
	public interface IElasticDatabaseClient
	{
		void SaveRawTickerData(string pair, object tickerResponse, Type responseType, DateTime time);
		void SaveSanitizedDate(TickerFormattedDto formattedDto);
		GetFormattedDataResponse GetFormattedTickers(GetFormattedDataRequest request);
		void SaveEcoIndexData(EcoIndex ecoIndex);
		List<EcoIndex> GetLastEcoIndex(string pattern, EcoIndexEnum ecoIndex, string pairName, int size = 1);
	}
}

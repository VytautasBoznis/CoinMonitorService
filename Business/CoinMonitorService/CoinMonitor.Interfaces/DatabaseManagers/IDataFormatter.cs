namespace CoinMonitor.Interfaces.DatabaseManagers
{
	public interface IDataFormatter
	{
		string GetApiPairName(string symbol1, string symbol2);
	}
}

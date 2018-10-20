namespace CoinMonitor.Interfaces.OutsideApiManagers
{
	public interface IBaseOutsideApiManager
	{
		void GetMarketData();
		void GetPairHistory();
	}
}

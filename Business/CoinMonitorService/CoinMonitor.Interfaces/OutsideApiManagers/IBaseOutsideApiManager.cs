namespace CoinMonitor.Interfaces.OutsideApiManagers
{
	public interface IBaseOutsideApiManager
	{
		void GetPayrData();
		void GetPayrHistory();
		void PlacePayrOrder();
		void CancelPayrOrder();
	}
}

using CoinMonitor.Interfaces.OutsideApiManagers;
using log4net;

namespace CoinMonitor.Business.Managers.Base
{
	public abstract class MarketWatchManager: IBaseOutsideApiManager
	{
		protected ILog Logger { get; }

		protected MarketWatchManager()
		{
			Logger = LogManager.GetLogger(GetType().Name);
		}

		public virtual void GetMarketData()
		{
			throw new System.NotImplementedException();
		}

		public  virtual void GetPairHistory()
		{
			throw new System.NotImplementedException();
		}
	}
}

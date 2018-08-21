using System.ServiceProcess;
using log4net;

namespace CoinMonitor.Base.Service
{
	public abstract class BaseServiceHost: ServiceBase
	{
		public ILog Logger;

		protected BaseServiceHost()
		{
			Logger = LogManager.GetLogger(GetType().Name);
		}
	}
}

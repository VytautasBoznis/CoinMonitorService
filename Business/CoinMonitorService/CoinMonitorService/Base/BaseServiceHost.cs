using System.ServiceProcess;
using log4net;

namespace CoinMonitorService.Base
{
	public abstract class BaseServiceHost: ServiceBase
	{
		private static ILog _logger;

		protected ILog Logger => _logger;

		protected BaseServiceHost()
		{
			_logger = LogManager.GetLogger(GetType().Name);
		}
	}
}

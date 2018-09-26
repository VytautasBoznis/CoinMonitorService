using log4net;

namespace CoinMonitor.Business.ServiceHolders
{
	public class BaseServiceHolder
	{
		private static ILog _logger;

		protected ILog Logger => _logger;

		public BaseServiceHolder()
		{
			_logger = LogManager.GetLogger(GetType().Name);
		}
	}
}

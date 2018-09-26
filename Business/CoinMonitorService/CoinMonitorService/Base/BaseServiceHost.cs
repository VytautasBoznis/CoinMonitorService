using System.Collections.Generic;
using System.ServiceProcess;
using CoinMonitor.Interfaces.ServiceHolders;
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
			ServiceHolders = new List<IServiceHolder>();
		}

		public void RegisterHolder(IServiceHolder holder)
		{
			ServiceHolders.Add(holder);
		}

		public List<IServiceHolder> ServiceHolders { get; }
	}
}

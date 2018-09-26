using CoinMonitor.Business.ServiceHolders;
using CoinMonitor.Interfaces.ServiceHolders;
using CoinMonitorService.Base;

namespace CoinMonitorService
{
	public partial class CoinMonitorServiceHost : BaseServiceHost
	{
		public CoinMonitorServiceHost()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			Logger.DebugFormat($"Starting service");
			Logger.DebugFormat($"Starting holders");

			ServiceHolders.Add(new MarketWatchServiceHolder());

			Logger.DebugFormat($"Initializing holders");

			foreach (IServiceHolder serviceHolder in ServiceHolders)
			{
				serviceHolder.Init();
			}
		}

		protected override void OnStop()
		{
			Logger.DebugFormat($"Stoping holders");

			foreach (IServiceHolder serviceHolder in ServiceHolders)
			{
				serviceHolder.Stop();
			}

			Logger.DebugFormat($"Stoping service");
		}
	}
}

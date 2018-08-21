using CoinMonitor.Base.Service;

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

		}

		protected override void OnStop()
		{
			Logger.DebugFormat($"Stoping service");
		}
	}
}

using System;
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

			try
			{
				foreach (IServiceHolder serviceHolder in ServiceHolders)
				{
					serviceHolder.Init();
				}
			}
			catch (Exception e)
			{
				Logger.ErrorFormat($"Error on itialization: {e.Message} stactrace: {e.StackTrace}");
			}
			
		}

		protected override void OnStop()
		{
			Logger.DebugFormat($"Stoping holders");
			try
			{
				foreach (IServiceHolder serviceHolder in ServiceHolders)
				{
					serviceHolder.Stop();
				}
			}
			catch (Exception e)
			{
				Logger.ErrorFormat($"Error on stop: {e.Message} stactrace: {e.StackTrace}");
			}

			Logger.DebugFormat($"Stoping service");
		}
	}
}

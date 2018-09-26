using System;
using System.Threading;
using CoinMonitor.Business.Holders;
using CoinMonitor.Business.Providers;
using CoinMonitor.Interfaces.ServiceHolders;

namespace CoinMonitor.Business.ServiceHolders
{
	public class MarketWatchServiceHolder: BaseServiceHolder, IServiceHolder, IMarketWatchHolder
	{
		private readonly TimeSpan _zeroTimespan = TimeSpan.Zero;
		private TimeSpan _pairSynchTimespan;
		private TimeSpan _marketSynchTimespan;
		private Timer MarketSynchTimer { get; set; }
		private Timer PriceSynchTimer { get; set; } 
		
		public void Init()
		{
			Logger.DebugFormat($"MarketWatchServiceHolder ONLINE");
			LoadConfigs();

			MarketSynchTimer = new Timer(e =>
			{
				MarketSynch();
			}, null, _zeroTimespan, _marketSynchTimespan);

			PriceSynchTimer = new Timer(e =>
			{
				PriceSynch();
			}, null, _zeroTimespan, _pairSynchTimespan);
		}

		public void Stop()
		{
			MarketSynchTimer.Dispose();
			PriceSynchTimer.Dispose();
		}

		public void MarketSynch()
		{
			Logger.DebugFormat($"Market synch");
		}

		public void PriceSynch()
		{
			Logger.DebugFormat($"Market synch");
		}

		#region Private methods

		private void LoadConfigs()
		{
			_pairSynchTimespan = ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.PairSynchTimeSeconds, TimeSpan.Parse("10"));
			_marketSynchTimespan = ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.MarketSynchTimeSeconds, TimeSpan.Parse("60"));
		}
		
		#endregion
	}
}

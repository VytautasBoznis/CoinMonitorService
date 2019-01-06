using System;
using System.Collections.Generic;
using System.Threading;
using CoinMonitor.Business.Managers;
using CoinMonitor.Business.Managers.Market;
using CoinMonitor.Core.Holders;
using CoinMonitor.Core.Providers;
using CoinMonitor.Interfaces.OutsideApiManagers;
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
		private readonly List<IBaseOutsideApiManager> _marketManagers;

		public MarketWatchServiceHolder()
		{
			_marketManagers = new List<IBaseOutsideApiManager>();
		}

		public void Init()
		{
			Logger.DebugFormat($"MarketWatchServiceHolder ONLINE");
			LoadConfigs();
			RegisterMarketManagers();

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

			Logger.DebugFormat($"All timers disposed");
		}

		public void MarketSynch()
		{
			try
			{
				foreach (var manager in _marketManagers)
				{
					new Thread(manager.GetMarketData).Start();
				}
			}
			catch (Exception e)
			{
				Logger.ErrorFormat($"Error happend tryin to synch markets: {e.Message} stacktrace: {e.StackTrace}");
			}
		}

		public void PriceSynch()
		{
			Logger.DebugFormat($"Price synch");
		}

		#region Private methods

		private void LoadConfigs()
		{
			_pairSynchTimespan = ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.PairSynchTimeSpan, TimeSpan.Parse("0:0:10"));
			_marketSynchTimespan = ConfigurationProvider.GetConfigurationByName(ConfigurationNameHolder.MarketSynchTimeSpan, TimeSpan.Parse("0:1:0"));
		}

		private void RegisterMarketManagers()
		{
			_marketManagers.Add(new CexMarketManager());
			_marketManagers.Add(new PoloniexMarketManager());
		}

		#endregion
	}
}

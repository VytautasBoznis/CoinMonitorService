using CoinMonitor.Business.Operations.CoinOperations;
using CoinMonitor.Domain.Messages.Wcf;
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
			
			GetCoinPairsRequest request = new GetCoinPairsRequest();
			new GetCoinPairsOperation().Excecute(request);

		}

		protected override void OnStop()
		{
			Logger.DebugFormat($"Stoping service");
		}
	}
}

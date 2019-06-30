using CoinMonitor.Domain.Messages.Base;

namespace CoinMonitor.Interfaces.OutsideApiManagers
{
	public interface ICoinMonitoringApiManager
	{
		BaseRestResponse InvokeRecalculation(BaseRestRequest request);
	}
}

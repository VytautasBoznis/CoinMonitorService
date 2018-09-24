using System.ServiceProcess;

namespace CoinMonitorService
{
	static class Program
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			var servicesToRun = new ServiceBase[]
			{
				new CoinMonitorServiceHost()
			};

			ServiceBase.Run(servicesToRun);
		}
	}
}

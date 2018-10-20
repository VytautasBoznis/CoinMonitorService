using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace CoinMonitorService
{
	static class Program
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{

			if (Environment.UserInteractive)
			{
				string parameter = string.Concat(args);
				switch (parameter)
				{
					case "--install":
						ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
						break;
					case "--uninstall":
						ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
						break;
				}
			}
			else
			{
				var servicesToRun = new ServiceBase[]
				{
					new CoinMonitorServiceHost()
				};
				ServiceBase.Run(servicesToRun);
			}
		}
	}
}

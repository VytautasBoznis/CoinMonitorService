using System;
using System.Diagnostics;
using log4net;

namespace CoinMonitor.Core.Timers
{
	public class OperationTimer: IDisposable
	{
		private readonly Stopwatch _stopwatch;
		private readonly string _operationName;
		private readonly ILog _logger;

		public OperationTimer(Type typeName, string operationName)
		{
			_stopwatch = new Stopwatch();
			_logger = LogManager.GetLogger(typeName);
			_operationName = operationName;
			_logger.DebugFormat($"Operation {_operationName} started");
			_stopwatch.Start();
		}

		public void Dispose()
		{
			_stopwatch.Stop();
			TimeSpan elapsed = _stopwatch.Elapsed;
			_logger.DebugFormat($"Operation {_operationName} ended time elapsed {elapsed}");
		}
	}
}

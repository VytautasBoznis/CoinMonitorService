using CoinMonitor.Base.Messages;
using log4net;

namespace CoinMonitor.Business.Operations.Base
{
	public abstract class BaseOperation<TBaseWcfRequest, TBaseWcfResponse> where TBaseWcfRequest : BaseWcfRequest
																		   where TBaseWcfResponse : BaseWcfResponse
	{
		private static ILog _logger;
		public ILog Logger => _logger;

		protected BaseOperation()
		{
			_logger = LogManager.GetLogger(GetType().Name);
		}

		public virtual TBaseWcfResponse Excecute(TBaseWcfRequest request)
		{
			return default(TBaseWcfResponse);
		}
	}
}

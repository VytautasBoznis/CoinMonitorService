using CoinMonitor.Business.Operations.Base;
using CoinMonitor.Domain.Messages.Wcf;

namespace CoinMonitor.Business.Operations.CoinOperations
{
	public class GetCoinPairsOperation: BaseOperation<GetCoinPairsRequest, GetCoinPairsResponse>
	{
		public GetCoinPairsOperation()
		{
		}

		public override GetCoinPairsResponse Excecute(GetCoinPairsRequest request)
		{
			Logger.WarnFormat("TESTAS");
			return new GetCoinPairsResponse();
		}
	}
}

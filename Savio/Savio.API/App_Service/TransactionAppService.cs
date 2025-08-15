using Abp.Application.Services;
using Savio.API;
using Savio.Core.Data;
using Transaction.API;
using Transaction.Contract;

namespace Transaction.API.App_Service
{
    public interface ITransactionAppService : IApplicationService
    {
        Tuple<int, List<TransactionModel>> GetAllTransactionsWithData(TransactionModel request);
        int AddEditTransaction(TransactionModel request);
        int DeleteTransactionById(int id);
    }
    public class TransactionAppService : ITransactionAppService
    {
        private readonly ITransactionService _transactionService;

        public TransactionAppService()
        {
            _transactionService = GlobalContext.TransactionService;
        }

        public Tuple<int, List<TransactionModel>> GetAllTransactionsWithData(TransactionModel request)
        {
            var r = _transactionService.GetAllTransactionsWithData(request);
            return new Tuple<int, List<TransactionModel>>(r.Item1, r.Item2);
        }

        public int AddEditTransaction(TransactionModel request)
        {
            return _transactionService.InsertTransaction(request);
        }

        public int DeleteTransactionById(int id)
        {
            return _transactionService.DeleteTransactionById(id);
        }

    }
}

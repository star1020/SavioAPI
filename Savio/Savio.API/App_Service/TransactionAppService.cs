using Abp.Application.Services;
using Savio.API;
using Savio.Core.Data;
using Transaction.API;
using Transaction.Contract;

namespace Transaction.API.App_Service
{
    public interface ITransactionAppService : IApplicationService
    {
        Tuple<int, List<TransactionModel>> GetAllTransactions();
        int AddEditTransaction(TransactionModel request);
        Tuple<int, TransactionModel> GetTransactionById(int id);
        int DeleteTransactionById(int id);
    }
    public class TransactionAppService : ITransactionAppService
    {
        private readonly ITransactionService _transactionService;

        public TransactionAppService()
        {
            _transactionService = GlobalContext.TransactionService;
        }

        public Tuple<int, List<TransactionModel>> GetAllTransactions()
        {
            var r = _transactionService.GetAllTransactions();
            return new Tuple<int, List<TransactionModel>>(r.Item1, r.Item2);
        }

        public int AddEditTransaction(TransactionModel request)
        {
            return _transactionService.InsertTransaction(request);
        }

        public Tuple<int, TransactionModel> GetTransactionById(int id)
        {
            var r = _transactionService.GetTransactionById(id);
            return new Tuple<int, TransactionModel>(r.Item1, r.Item2);
        }

        public int DeleteTransactionById(int id)
        {
            return _transactionService.DeleteTransactionById(id);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Savio.Core.Data;

namespace Transaction.Contract
{
    [ServiceContract]
    public interface ITransactionService
    {
        [OperationContract]
        Tuple<int, List<TransactionModel>> GetAllTransactions();
        [OperationContract]
        int InsertTransaction(TransactionModel user);
        [OperationContract]
        Tuple<int, TransactionModel> GetTransactionById(int id);
        [OperationContract]
        int DeleteTransactionById(int id);
    }
}

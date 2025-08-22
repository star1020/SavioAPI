using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Savio.Core;
using Savio.Core.Data;
using Transaction.Contract;

namespace Transaction
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _db;
        public string className = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public TransactionService(ITransactionRepository db)
        {
            _db = db;
        }

        public Tuple<int, List<TransactionModel>> GetAllTransactionsWithData(TransactionModel transaction)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogHelper.Logging(className, method, transaction, true);
            var r = _db.GetAllTransactionsWithData(transaction);
            LogHelper.Logging(className, method, r, false);

            if (r == null) return new Tuple<int, List<TransactionModel>>(ErrorCode.OperationError, new List<TransactionModel>());
            if (r.Count == 0) return new Tuple<int, List<TransactionModel>>(ErrorCode.OperationError, new List<TransactionModel>());

            return new Tuple<int, List<TransactionModel>>(ErrorCode.Success, r);
        }

        public int InsertTransaction(TransactionModel transaction)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogHelper.Logging(className, method, transaction, true);
            var r = _db.InsertTransaction(transaction);
            LogHelper.Logging(className, method, r, false);
            return r;
        }

        public int DeleteTransactionById(int id)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogHelper.Logging(className, method, id, true);
            var r = _db.DeleteTransactionById(id);
            LogHelper.Logging(className, method, r, false);
            return r;
        }

    }
}

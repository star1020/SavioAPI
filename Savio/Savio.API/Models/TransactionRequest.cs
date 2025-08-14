using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transaction.API.Models
{
    public class TransactionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<TransactionModel> Transaction { get; set; }
    }

    public class GetTransactionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public TransactionModel Transaction { get; set; }
    }

}
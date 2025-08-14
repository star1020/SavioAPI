using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savio.Core.Data
{
    public class TransactionModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int category_id { get; set; }
        public string type { get; set; }
        public int member_id { get; set; }
        public decimal value { get; set; }
        public string action { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

}

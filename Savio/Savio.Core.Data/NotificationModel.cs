using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savio.Core.Data
{
    public class NotificationModel
    {
        public int id { get; set; }
        public string msg { get; set; }
        public int notification_type { get; set; }
        public int day_of_week { get; set; }
        public int day_of_month { get; set; }
        public string send_time { get; set; }
        public decimal value { get; set; }
        public string action { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

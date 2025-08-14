using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savio.Core.Data
{
    public class CategoryModel
    {
        public int id { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string action { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Category.API.Models
{
    public class CategoryResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<CategoryModel> Category { get; set; }
    }

    public class GetCategoryResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public CategoryModel Category { get; set; }
    }

}
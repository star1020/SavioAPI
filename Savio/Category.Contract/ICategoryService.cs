using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Savio.Core.Data;

namespace Category.Contract
{
    [ServiceContract]
    public interface ICategoryService
    {
        [OperationContract]
        Tuple<int, List<CategoryModel>> GetAllCategories();
        [OperationContract]
        int InsertCategory(CategoryModel user);
        [OperationContract]
        Tuple<int, CategoryModel> GetCategoryById(int id);
        [OperationContract]
        int DeleteCategoryById(int id);
    }
}

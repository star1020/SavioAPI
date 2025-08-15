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
        Tuple<int, List<CategoryModel>> GetAllCategoriesWithData(CategoryModel category);
        [OperationContract]
        int InsertCategory(CategoryModel user);
        [OperationContract]
        int DeleteCategoryById(int id);
    }
}

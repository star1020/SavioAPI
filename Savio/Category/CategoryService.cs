using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Category.Contract;
using Newtonsoft.Json;
using NLog;
using Savio.Core.Data;

namespace Category
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _db;

        public CategoryService(ICategoryRepository db)
        {
            _db = db;
        }


        public Tuple<int, List<CategoryModel>> GetAllCategories()
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> no data");
            var r = _db.GetAllCategories();
            LogManager.GetCurrentClassLogger().Info($"[{method}] CategoryInfo -> {JsonConvert.SerializeObject(r)}");

            if (r == null) return new Tuple<int, List<CategoryModel>>(ErrorCode.OperationError, new List<CategoryModel>());
            if (r.Count == 0) return new Tuple<int, List<CategoryModel>>(ErrorCode.OperationError, new List<CategoryModel>());

            return new Tuple<int, List<CategoryModel>>(ErrorCode.Success, r);

        }

        public int InsertCategory(CategoryModel transaction)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> {JsonConvert.SerializeObject(transaction)}");
            var r = _db.InsertCategory(transaction);
            LogManager.GetCurrentClassLogger().Info($"[{method}] Result -> {JsonConvert.SerializeObject(r)}");
            return r;
        }

        public Tuple<int, CategoryModel> GetCategoryById(int id)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> ${id}");
            var r = _db.GetCategoryById(id);
            LogManager.GetCurrentClassLogger().Info($"[{method}] CategoryInfo -> {JsonConvert.SerializeObject(r)}");

            if (r == null) return new Tuple<int, CategoryModel>(ErrorCode.OperationError, new CategoryModel());

            return new Tuple<int, CategoryModel>(ErrorCode.Success, r);

        }

        public int DeleteCategoryById(int id)
        {
            var method = MethodBase.GetCurrentMethod().Name;
            LogManager.GetCurrentClassLogger().Info($"[{method}] RequestInfo -> ${id}");
            var r = _db.DeleteCategoryById(id);
            LogManager.GetCurrentClassLogger().Info($"[{method}] CategoryInfo -> {JsonConvert.SerializeObject(r)}");
            return r;
        }
    }
}

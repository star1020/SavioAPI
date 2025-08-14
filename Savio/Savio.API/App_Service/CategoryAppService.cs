using Abp.Application.Services;
using Savio.API;
using Savio.Core.Data;
using Category.API;
using Category.Contract;

namespace Category.API.App_Service
{
    public interface ICategoryAppService : IApplicationService
    {
        Tuple<int, List<CategoryModel>> GetAllCategories();
        int AddEditCategory(CategoryModel request);
        Tuple<int, CategoryModel> GetCategoryById(int id);
        int DeleteCategoryById(int id);
    }
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryService _categoryService;

        public CategoryAppService()
        {
            _categoryService = GlobalContext.CategoryService;
        }

        public Tuple<int, List<CategoryModel>> GetAllCategories()
        {
            var r = _categoryService.GetAllCategories();
            return new Tuple<int, List<CategoryModel>>(r.Item1, r.Item2);
        }

        public int AddEditCategory(CategoryModel request)
        {
            return _categoryService.InsertCategory(request);
        }

        public Tuple<int, CategoryModel> GetCategoryById(int id)
        {
            var r = _categoryService.GetCategoryById(id);
            return new Tuple<int, CategoryModel>(r.Item1, r.Item2);
        }

        public int DeleteCategoryById(int id)
        {
            return _categoryService.DeleteCategoryById(id);
        }

    }
}

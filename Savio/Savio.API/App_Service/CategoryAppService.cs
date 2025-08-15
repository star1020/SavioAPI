using Abp.Application.Services;
using Savio.API;
using Savio.Core.Data;
using Category.API;
using Category.Contract;

namespace Category.API.App_Service
{
    public interface ICategoryAppService : IApplicationService
    {
        Tuple<int, List<CategoryModel>> GetAllCategoriesWithData(CategoryModel request);
        int AddEditCategory(CategoryModel request);
        int DeleteCategoryById(int id);
    }
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryService _categoryService;

        public CategoryAppService()
        {
            _categoryService = GlobalContext.CategoryService;
        }
        public Tuple<int, List<CategoryModel>> GetAllCategoriesWithData(CategoryModel request)
        {
            var r = _categoryService.GetAllCategoriesWithData(request);
            return new Tuple<int, List<CategoryModel>>(r.Item1, r.Item2);
        }

        public int AddEditCategory(CategoryModel request)
        {
            return _categoryService.InsertCategory(request);
        }

        public int DeleteCategoryById(int id)
        {
            return _categoryService.DeleteCategoryById(id);
        }

    }
}

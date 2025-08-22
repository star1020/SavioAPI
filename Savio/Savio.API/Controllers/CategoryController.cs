using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Savio.Core;
using Savio.Core.Data;
using Category.API.App_Service;
using Category.API.Models;

namespace Category.API.Controllers
{
    [ApiController]
    [Route("category")]
    public class CategoryController : ControllerBase
    {
        readonly ICategoryAppService _categoryAppService;
        public readonly string _controllerName;
        public CategoryController()
        {
            _categoryAppService = new CategoryAppService();
            _controllerName = GetType().Name;
        }

        [HttpGet]
        [HttpPost]
        [Route("GetAllCategoriesWithData")]
        public IActionResult GetAllCategoriesWithData(CategoryModel request)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, request, true);

            var r = _categoryAppService.GetAllCategoriesWithData(request);

            var response = new CategoryResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                Category = r.Item2
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }

        [HttpPost]
        [Route("AddEditCategory")]
        public IActionResult AddEditCategory(CategoryModel request)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, request, true);

            var r = _categoryAppService.AddEditCategory(request);

            var response = new CategoryResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }

        [HttpPost]
        [Route("DeleteCategoryById/{id:int}")]
        public IActionResult DeleteCategoryById(int id)
        {
            var w = Stopwatch.StartNew();
            string method = ControllerContext.ActionDescriptor.ActionName;

            LogHelper.Logging(_controllerName, method, id, true);

            var r = _categoryAppService.DeleteCategoryById(id);

            var response = new CategoryResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogHelper.Logging(_controllerName, method, response, false);
            return Ok(response);
        }
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Savio.Core;
using Savio.Core.Data;
using Category.API.App_Service;
using Category.API.Models;
using Category.API.Models;

namespace Category.API.Controllers
{
    [ApiController]
    [Route("category")]
    public class CategoryController : ControllerBase
    {
        readonly ICategoryAppService _categoryAppService;
        public CategoryController()
        {
            _categoryAppService = new CategoryAppService();
        }

        [HttpGet]
        [HttpPost]
        [Route("GetAllCategoriesWithData")]
        public IActionResult GetAllCategoriesWithData(CategoryModel request)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> {request}");

            var r = _categoryAppService.GetAllCategoriesWithData(request);

            var response = new CategoryResponse
            {
                Code = r.Item1,
                Message = r.Item1.ToErrorMsg(),
                Category = r.Item2
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }

        [HttpPost]
        [Route("AddEditCategory")]
        public IActionResult AddEditCategory(CategoryModel request)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> {request}");

            var r = _categoryAppService.AddEditCategory(request);

            var response = new CategoryResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }

        [HttpPost]
        [Route("DeleteCategoryById/{id:int}")]
        public IActionResult DeleteCategoryById(int id)
        {
            var w = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            string method = LogHelper.GetMethodName();

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Request -> {id}");

            var r = _categoryAppService.DeleteCategoryById(id);

            var response = new CategoryResponse
            {
                Code = r,
                Message = r.ToErrorMsg(),
            };

            LogManager.GetCurrentClassLogger().Info($"[{correlationId}] {method} Response -> {JsonConvert.SerializeObject(response)} | TotalProcessedTimeMls = [{w.ElapsedMilliseconds}]");

            return Ok(response);
        }
    }
}

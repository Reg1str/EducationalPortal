using EducationPortal.Application.Services.DTO;
using EducationPortal.Application.Services.Interfaces;
using EducationPortal.Domain.Core.Common;
using EducationProject.MVC.Managers;
using EducationProject.MVC.Models.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Controllers
{
    public class MaterialController : Controller
    {
        private readonly SignInManager _signInManager;
        private readonly IMaterialService _materialService;
        private readonly ISkillService _skillService;

        public MaterialController(
            SignInManager signInManager,
            IMaterialService materialService,
            ISkillService skillService)
        {
            _signInManager = signInManager;
            _materialService = materialService;
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<IActionResult> Page(int? courseId, int pageNumber = 1)
        {
            var pageInfo = new PageInfo()
            {
                PageNumber = pageNumber,
                PageSize = 10
            };

            if (courseId.HasValue)
            {
                ViewData["courseId"] = courseId.Value;
                var materialsAvailableResult =
                    await _materialService.GetAvailableMaterialsAsync(courseId.Value, pageInfo);

                if (materialsAvailableResult.IsSuccessful)
                {
                    return View(materialsAvailableResult.Value);
                }
            }
            else
            {
                var materialPageResult =
                    await _materialService.GetMaterialPageAsync(pageInfo);

                if (materialPageResult.IsSuccessful)
                {
                    return View(materialPageResult.Value);
                }
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<JsonResult> PageJson(int pageNumber = 1)
        {
            var pageInfo = new PageInfo()
            {
                PageNumber = pageNumber,
                PageSize = 2
            };

            var skillPageResult =
                await _materialService.GetMaterialPageAsync(pageInfo);

            if (skillPageResult.IsSuccessful)
            {
                return Json(skillPageResult.Value);
            }

            return Json(string.Empty);
        }
    
        [HttpGet]
        [Authorize]
        public IActionResult Create(string? materialType)
        {
            if (!string.IsNullOrEmpty(materialType))
            {
                ViewData["materialType"] = materialType;
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateArticle(ArticleModel articleModel)
        {
            var articleDTO = new ArticleDTO()
            {
                Title = articleModel.Title,
                Description = articleModel.Description,
                SourceUrl = articleModel.SourceUrl,
                PublishedDate = articleModel.PublishedDate,
                Type = "Article"
            };

            var createArticleResult =
                await _materialService.CreateMaterialAsync(articleDTO);

            if (createArticleResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Material");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePrintedMaterial(PrintedMaterialModel printedMaterialModel)
        {
            var printedMaterialDTO = new PrintedMaterialDTO()
            {
                Title = printedMaterialModel.Title,
                Description = printedMaterialModel.Description,
                PagesCount = printedMaterialModel.PagesCount,
                AuthorNames = new List<string>() { printedMaterialModel.AuthorName },
                Type = "PrintedMaterial"
            };

            var createArticleResult =
                await _materialService.CreateMaterialAsync(printedMaterialDTO);

            if (createArticleResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Material");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVideo(VideoModel videoModel)
        {
            var videoDTO = new VideoDTO()
            {
                Title = videoModel.Title,
                Description = videoModel.Description,
                Length = videoModel.Length,
                Quality = videoModel.Quality,
                Type = "Video"
            };

            var createArticleResult =
                await _materialService.CreateMaterialAsync(videoDTO);

            if (createArticleResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Material");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Finish(int materialId)
        {
            var finishMaterialResult =
                await _materialService.FinishMaterial(materialId, _signInManager.GetUserId());

            var updateCourseProgressResult =
                await _skillService.UpdateSkillProgressAsync(materialId, _signInManager.GetUserId());

            if (finishMaterialResult.IsSuccessful && updateCourseProgressResult.IsSuccessful)
            {
                return RedirectToAction("MaterialInfo", "Material", new { materialId = materialId });
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> MaterialInfo(int materialId)
        {
            var materialInfoDtoResult =
                await _materialService.GetMaterialInfoAsync(materialId, _signInManager.GetUserId());

            if (materialInfoDtoResult.IsSuccessful)
            {
                return View(materialInfoDtoResult.Value);
            }

            return View();
        }
    }
}

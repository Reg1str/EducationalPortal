using EducationPortal.Application.Services.DTO;
using EducationPortal.Application.Services.Interfaces;
using EducationPortal.Domain.Core.Common;
using EducationProject.MVC.Models.Skill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Controllers
{
    public class SkillController : Controller
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
                var skillPageResult =
                    await _skillService.GetSkillsAvailableForCourseAsync(courseId.Value, pageInfo);

                if (skillPageResult.IsSuccessful)
                {
                    return View(skillPageResult.Value);
                }
            }
            else
            {
                var skillPageResult =
                    await _skillService.GetSkillPage(pageInfo);

                if (skillPageResult.IsSuccessful)
                {
                    return View(skillPageResult.Value);
                }
            }

            return View();
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
                await _skillService.GetSkillPage(pageInfo);

            if (skillPageResult.IsSuccessful)
            {
                return Json(skillPageResult.Value);
            }

            return Json(string.Empty);
        }

        [HttpPost] 
        [Authorize]
        public async Task<IActionResult> Create(SkillCreateModel skillCreateModel)
        {
            var skillCreateDTO = new SkillCreateDTO()
            {
                Name = skillCreateModel.Name,
                Penalty = skillCreateModel.Penalty,
                MaxLevel = skillCreateModel.MaxLevel
            };

            var createSkillResult =
                await _skillService.CreateSkillAsync(skillCreateDTO);
             
            if (createSkillResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Skill");
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
    }
}

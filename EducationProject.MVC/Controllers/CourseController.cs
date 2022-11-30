using EducationPortal.Application.Services.DTO;
using EducationPortal.Application.Services.Interfaces;
using EducationPortal.Domain.Core.Common;
using EducationProject.MVC.Managers;
using EducationProject.MVC.Models.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly SignInManager _signInManager;
        private readonly ICourseService _courseService;

        public CourseController(
            SignInManager signInManager,
            ICourseService courseService)
        {
            _signInManager = signInManager;
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> Page(int pageNumber = 1)
        {
            var pageInfo = new PageInfo()
            {
                PageNumber = pageNumber,
                PageSize = 10
            };

            var coursePageInfoDTOResult =
                await _courseService.GetCoursePage(pageInfo);

            if (coursePageInfoDTOResult.IsSuccessful)
            {
                var coursePageInfoDTO = coursePageInfoDTOResult.Value;

                var courseInfoPageModel = new CoursePageModel()
                {
                    CourseInfoDTOs = coursePageInfoDTO.CourseInfoDTOs,
                    LastPageNumber = coursePageInfoDTO.LastPageNumber,
                    PageNumber = pageNumber
                };

                return View(courseInfoPageModel);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> CourseInfo(int courseId, int pageNumber = 1)
        {
            var pageInfo = new PageInfo()
            {
                PageNumber = pageNumber,
                PageSize = 10
            };

            var courseInfoResult =
                await _courseService.GetCourseInfoForUserAsync(
                    courseId, _signInManager.GetUserId(), _signInManager.IsSignedIn(), pageInfo);

            if (courseInfoResult.IsSuccessful)
            {
                var courseInfo = courseInfoResult.Value;
                return View(courseInfo);
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string Title, string Description, List<int> MaterialIds, List<int> SkillIds)
        {
            var courseCreateDTO = new CourseCreateDTO()
            {
                UserOwnerId = _signInManager.GetUserId(),
                CourseTitle = Title,
                CourseDescription = Description,
                CourseMaterialIds = MaterialIds,
                SkillIds = SkillIds
            };

            var courseCreateResult =
                await _courseService.CreateCourseAsync(courseCreateDTO);

            if (courseCreateResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Course");
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int courseId)
        {
            var courseEditResult =
                await _courseService.GetCourseEditDtoAsync(courseId);

            if (courseEditResult.IsSuccessful)
            {
                return View(courseEditResult.Value);
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CourseEditModel courseEditModel)
        {
            var courseEditDto = new CourseEditDTO()
            {
                Id = courseEditModel.Id,
                Title = courseEditModel.Title,
                Description = courseEditModel.Description
            };

            var courseEditResult =
                await _courseService.EditCourseInfo(courseEditDto);

            if (courseEditResult.IsSuccessful)
            {
                return RedirectToAction("CourseInfo", "Course", new { courseId = courseEditModel.Id });
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Enroll(int courseId, int userId)
        {
            var userEnrollmentDTO = new UserEnrollmentDTO()
            {
                CourseId = courseId,
                UserId = userId
            };

            var enrollmentResult =
                await _courseService.AddUserEnrollmentToCourse(userEnrollmentDTO);

            if (enrollmentResult.IsSuccessful)
            {
                return RedirectToAction("CourseInfo", "Course", new { courseId = courseId});
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMaterial(int courseId, int materialId)
        {
            var addMaterialsToCourseDTO = new AddMaterialsToCourseDTO()
            {
                CourseId = courseId,
                MaterialIds = new List<int>() { materialId }
            };

            var addMaterialsToCourseResult =
                await _courseService.AddMaterialsToCourse(addMaterialsToCourseDTO);


            if (addMaterialsToCourseResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Material", new { courseId = courseId });
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveMaterial(int courseId, int materialId)
        {
            var removeMaterialFromCourseDTO = new RemoveMaterialFromCourseDTO()
            {
                CourseId = courseId,
                MaterialId = materialId
            };
            
            var removeMaterialFromCourseResult =
                await _courseService.RemoveMaterialFromCourse(removeMaterialFromCourseDTO);

            if (removeMaterialFromCourseResult.IsSuccessful)
            {
                return RedirectToAction("CourseInfo", "Course", new { courseId = courseId }); 
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddSkill(int courseId, int skillId)
        {
            var skillCourseDTO = new SkillCourseDTO()
            {
                CourseId = courseId,
                SkillId = skillId
            };

            var addSkillToCourseResult =
                await _courseService.AddSkillToCourseAsync(skillCourseDTO);

            if (addSkillToCourseResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Skill", new { courseId = courseId });
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveSkill(int courseId, int skillId)
        {
            var skillCourseDTO = new SkillCourseDTO()
            {
                CourseId = courseId,
                SkillId = skillId
            };

            var removeSkillFromCourseResult =
                await _courseService.RemoveSkillFromCourseAsync(skillCourseDTO);

            if (removeSkillFromCourseResult.IsSuccessful)
            {
                return RedirectToAction("CourseInfo", "Course", new { courseId = courseId });
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int courseId)
        {
            var deleteCourseResult =
                await _courseService.DeleteCourseAsync(courseId);

            if (deleteCourseResult.IsSuccessful)
            {
                return RedirectToAction("Page", "Course");
            }

            return BadRequest();
        }
    }
}

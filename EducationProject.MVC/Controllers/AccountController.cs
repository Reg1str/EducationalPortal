namespace EducationProject.MVC.Controllers
{
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Application.Services.Interfaces;
    using EducationProject.MVC.Managers;
    using EducationProject.MVC.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IUserService userService;
        private readonly ISkillService skillService;
        private readonly SignInManager signInManager;

        public AccountController(
            ICourseService courseService,
            IUserService userService,
            ISkillService skillService,
            SignInManager signInManager)
        {
            this.courseService = courseService;
            this.skillService = skillService;
            this.userService = userService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var getUserResult =
                    await this.userService.GetAsync<UserInfoDTO>(
                        x => x.Email == loginModel.Email && x.Password == loginModel.Password,
                        x => new UserInfoDTO()
                        {
                            Id = x.Id,
                            FirstName = x.Firstname,
                            LastName = x.Lastname,
                            Email = x.Email,
                            UserType = x.UserType
                        });

                if (getUserResult.IsSuccessful)
                {
                    var userInfoDTO = getUserResult.Value.FirstOrDefault();

                    if (userInfoDTO != null)
                    {
                        await this.signInManager.SignInAsync(userInfoDTO, true);
                        return RedirectToAction("Index", "Home");
                    }

                    return View();
                }
                else
                {
                    this.ModelState.AddModelError("", getUserResult.Exception.Message);
                }
            }

            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (this.signInManager.IsSignedIn())
            {
                return Redirect($"{Request.Host}/{Request.PathBase}");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (this.ModelState.IsValid)
            {
                var createUserResult =
                    await this.userService.RegisterUserAsync(new UserRegisterDTO()
                    {
                        Email = registerModel.Email,
                        Password = registerModel.Password,
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName
                    });

                if (createUserResult.IsSuccessful)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    this.ModelState.AddModelError("", createUserResult.Exception.Message);
                }
            }

            return View(registerModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var getCoursesForUserResult =
                await this.courseService.GetCoursesForUser(signInManager.GetUserId());

            var getSkillsForUserResult =
                await this.skillService.GetSkillsForUserAsync(signInManager.GetUserId());

            var userInfo =
                await this.userService.GetUserInfoAsync(signInManager.GetUserId());

            if (getCoursesForUserResult.IsSuccessful && getSkillsForUserResult.IsSuccessful)
            {
                ViewData["courseInfoList"] = getCoursesForUserResult.Value;
                ViewData["skillInfoList"] = getSkillsForUserResult.Value;
            }

            return View(userInfo.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            await this.userService.ChangePasswordAsync(
                new ChangePasswordDTO()
                {
                    UserId = this.signInManager.GetUserId(),
                    NewPassword = changePassword.Password
                });
            return RedirectToAction("Profile", "Account");
        }
    }
}

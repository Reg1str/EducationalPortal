using EducationPortal.Application.Services.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationProject.MVC.Managers
{
    public class SignInManager
    {
		private readonly IHttpContextAccessor httpContextAccessor;

		public SignInManager(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

        private HttpContext HttpContext =>
			this.httpContextAccessor.HttpContext;

		public async Task<bool> SignInAsync(UserInfoDTO userInfoDTO, bool isPersistent)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, userInfoDTO.Email),
				new Claim(ClaimTypes.NameIdentifier, userInfoDTO.Id.ToString()),
				new Claim(ClaimTypes.Role, userInfoDTO.UserType)
			};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			await this.HttpContext.SignInAsync(
				principal,
				new AuthenticationProperties
				{
					IsPersistent = isPersistent
				});

			return true;
		}

		public Task SignOutAsync()
		{
			return this.HttpContext.SignOutAsync();
		}

		public bool IsSignedIn()
		{
			return this.HttpContext.User.Identity.IsAuthenticated;
		}

		public int GetUserId()
		{
			var claim = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (claim == null || !int.TryParse(claim.Value, out var id))
			{
				return 0;
			}

			return id;
		}
	}
}

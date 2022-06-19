using AutoMapper;
using LoginSystem.Helpers;
using LoginSystem.Models;
using LoginSystem.Models.Register;
using LoginSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LoginSystem.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public UsersController(
            ILoginService loginService)
        {
            _loginService = loginService;
         
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate(LoginSignUpRequest model)
        {
            var response = _loginService.Login(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(LoginSignUpRequest model)
        {
            _loginService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _loginService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _loginService.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            _loginService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _loginService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}

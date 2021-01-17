using Application.Identity;
using Application.Identity.Models;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/v1")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [ProducesResponseType(typeof(Result<UserDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [AllowAnonymous]
        [HttpPost("authenticate", Name = "authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateUserCommand command)
        {
            return _userService.Authenticate(command);
        }

        [ProducesResponseType(typeof(Result<UserDto>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Result))]
        [AllowAnonymous]
        [HttpPost("register", Name = "register")]
        public IActionResult Register([FromBody] RegisterUserCommand command)
        {
            command.SetupForCreation(Url, "get-user", x => new { username = x.Name });

            return _userService.Create(command);
        }

        [ProducesResponseType(typeof(Result<List<UserDto>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet(Name = "get-users")]
        public IActionResult GetAll()
        {
            return _userService.GetAll();
        }

        [ProducesResponseType(typeof(Result<UserDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpGet("{username}", Name = "get-user")]
        public IActionResult GetByName(string username)
        {
            return _userService.GetByName(username);
        }

        [ProducesResponseType(typeof(Result<UserDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Result))]
        [HttpPut("{id}", Name = "update-user")]
        public IActionResult Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            return _userService.Update(command);
        }

        [HttpDelete("{id}", Name = "delete-user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public IActionResult Delete(Guid id)
        {
            return _userService.Delete(id);
        }
    }
}

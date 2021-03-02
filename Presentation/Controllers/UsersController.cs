using Application.Common.Abstractions;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Presentation.Controllers
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

        [ProducesResponseType(typeof(Output<UserResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [AllowAnonymous]
        [HttpPost("authenticate", Name = "authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateUserCommand command)
        {
            return _userService.Authenticate(command);
        }

        [ProducesResponseType(typeof(Output<UserResponse>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(Output))]
        [AllowAnonymous]
        [HttpPost("register", Name = "register")]
        public IActionResult Register([FromBody] RegisterUserCommand command)
        {
            command.SetupForCreation(Url, "get-user", x => new { username = x.Name });

            return _userService.Create(command);
        }

        [ProducesResponseType(typeof(Output<List<UserResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpGet(Name = "get-users")]
        public IActionResult GetAll()
        {
            return _userService.GetAll();
        }

        [ProducesResponseType(typeof(Output<UserResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
        [HttpGet("{username}", Name = "get-user")]
        public IActionResult GetByName(string username)
        {
            return _userService.GetByName(username);
        }

        [ProducesResponseType(typeof(Output<UserResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(Output))]
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

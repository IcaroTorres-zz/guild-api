using Application.Identity.Models;
using Domain.Responses;
using System;

namespace Application.Identity
{
    public interface IUserService
    {
        IApiResult Authenticate(AuthenticateUserCommand command);
        IApiResult Create(RegisterUserCommand command);
        IApiResult GetAll();
        IApiResult GetByName(string name);
        IApiResult Update(UpdateUserCommand command);
        IApiResult Delete(Guid id);
    }
}

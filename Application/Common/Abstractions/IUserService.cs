using Application.Identity;
using System;

namespace Application.Common.Abstractions
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

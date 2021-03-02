using Application.Common.Abstractions;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Identity;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly IdentityContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterUserCommand> _registerModelValidator;
        private readonly IValidator<AuthenticateUserCommand> _authenticateModelValidator;

        public UserService(IdentityContext context, IConfiguration configuration, IMapper mapper,
            IValidator<RegisterUserCommand> registerModelValidator,
            IValidator<AuthenticateUserCommand> authenticateModelValidator)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _registerModelValidator = registerModelValidator;
            _authenticateModelValidator = authenticateModelValidator;
        }

        public IApiResult Authenticate(AuthenticateUserCommand command)
        {
            if (_authenticateModelValidator.Validate(command) is { } validation && !validation.IsValid)
            {
                return new FailValidationResult(validation.Errors.ToArray());
            }

            var user = _context.Users.SingleOrDefault(x => x.Name == command.Name);

            if (user == null || !VerifyPasswordHash(command.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new FailExecutionResult(HttpStatusCode.BadRequest, new Error("Auth", "Username or password is incorrect"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"]);
            var tokenExpirationMinutes = int.Parse(_configuration["TokenExpirationMinutes"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new SuccessResult(new
            {
                user.Id,
                user.Name,
                user.Email,
                Token = tokenString,
                ExpiresIn = tokenExpirationMinutes * 60
            });
        }

        public IApiResult GetAll()
        {
            var users = _context.Users;
            var models = _mapper.Map<List<UserResponse>>(users);
            return new SuccessResult(models);
        }

        public IApiResult GetByName(string userName)
        {
            var user = _context.Users.SingleOrDefault(x => x.Name == userName);

            return user is null
                ? new FailExecutionResult(HttpStatusCode.NotFound,
                    new Error("User", $"Record not found for user with given name \"{userName}\"."))
                : new SuccessResult(_mapper.Map<UserResponse>(user)) as IApiResult;
        }

        public IApiResult Create(RegisterUserCommand command)
        {
            if (_registerModelValidator.Validate(command) is { } validation && !validation.IsValid)
            {
                return new FailValidationResult(validation.Errors.ToArray());
            }

            var user = _mapper.Map<User>(command);
            CreatePasswordHash(command.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return new SuccessCreatedResult(new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }, command);
        }

        public IApiResult Update(UpdateUserCommand command)
        {
            var user = _context.Users.Find(command.Id);
            if (user == null)
            {
                return new FailExecutionResult(HttpStatusCode.NotFound, new Error("Auth", "User not found."));
            }

            if (!string.IsNullOrWhiteSpace(command.Name) && command.Name != user.Name)
            {
                if (_context.Users.Any(x => x.Name == command.Name))
                {
                    return new FailExecutionResult(errors: new Error("Auth", $"Username '{command.Name}' is already taken."));
                }

                user.Name = command.Name;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(command.Email)) user.Email = command.Email;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(command.Password))
            {
                CreatePasswordHash(command.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return new SuccessResult(new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            });
        }

        public IApiResult Delete(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return new SuccessResult(default, HttpStatusCode.NoContent);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null || string.IsNullOrWhiteSpace(password) || storedHash.Length != 64 || storedSalt.Length != 128) return false;

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}

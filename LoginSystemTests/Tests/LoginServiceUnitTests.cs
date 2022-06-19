using AutoMapper;
using FluentAssertions;
using GenFu;
using LoginSystem.Entities;
using LoginSystem.Helpers;
using LoginSystem.Persistence;
using LoginSystem.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace LoginSystemTests.Tests
{
    public class LoginServiceUnitTests
    {
        [Test]
        public void PasswordVerifies()
        {
            Mock<DataContext> dataContext = new Mock<DataContext>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            Mock<IJwtUtilities> jwtUtilities = new Mock<IJwtUtilities>();

            LoginService loginService = new LoginService(dataContext.Object, mapper.Object,jwtUtilities.Object);

            string password = "password";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            loginService.AuthenticateUser(passwordHash, password)
                .Should().BeTrue();
        }

        [Test]
        public void UserGeneratesTokens()
        {
            var someOptions = Options.Create(new AppSettings());
            JwtUtilities jwtUtilities = new JwtUtilities(someOptions);

            var user = A.New<User>();
            var token = jwtUtilities.GenerateToken(user);
            token.Should().NotBeNull();
        }
    }
}

using AutoMapper;
using FluentAssertions;
using GenFu;
using LoginSystem.Helpers;
using LoginSystem.Maps;
using LoginSystem.Models;
using LoginSystem.Models.Register;
using LoginSystem.Persistence;
using LoginSystem.Services;
using LoginSystemTests.Helpers;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LoginSystemTests.Tests
{
    internal class LoginIntergrationTests
    {
        DataContext dataContext;
        LoginSignUpRequest request;
        Mapper mapper;
        LoginService loginService;
        JwtUtilities jwtUtilities;

        [OneTimeSetUp]
        public void SetUpTests()
        {
            var someOptions = Options.Create(new AppSettings());

            request = A.New<LoginSignUpRequest>();
            dataContext = new InMemoryDatabaseHelper()
                .DataContext;
            mapper = CreateMap();
            jwtUtilities = new JwtUtilities(someOptions);
            loginService = new LoginService(dataContext, mapper, jwtUtilities);
        }

        [Test, Order(1)]
        public void UserRegistersSuccesfully()
        {
            loginService.Register(request);

            var result = dataContext.Users.Where(x => x.Username == request.Username)
                .FirstOrDefault();

            result.Should().NotBeNull();
            result.Username.Should().Be(request.Username);
            result.Email.Should().Be(request.Email);
        }

        [Test, Order(2)]
        public void UserAlreadyRegistered()
        {
            loginService.VerifyDuplicateUser(request).Should().BeTrue();
        }

        [Test, Order(3)]
        public void UserLogsIn()
        {
            loginService.Login(request);
        }
        [Test, Order(4)]
        public void UserUpdates()
        {
            var updateModel = A.New<UpdateRequest>();

            loginService.Update(dataContext.Users.First().Id, updateModel);
            loginService
                .FindUserWithUserName(updateModel.Username)
                .Username
                .Should()
                .NotBeNull();
        }
        [Test, Order(5)]
        public void UserDeletes()
        {
            var user = dataContext.Users.First();

            loginService.Delete(user.Id);
            loginService.FindUserWithUserName(user.Username)
                .Should()
                .BeNull();
        }
        private static Mapper CreateMap()
        {
            var p = new List<Profile>
            {
                new RegisterRequestToUserProfile(),
                new AutoMapperProfile()
            };

            var configuration = new MapperConfiguration(cfg =>
            p.ForEach(p => cfg.AddProfile(p))
            );

            return new Mapper(configuration);
        }

    }
}

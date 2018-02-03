using System;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using ServiceStackAuth.ServiceModel;
using ServiceStackAuth.ServiceInterface;
using ServiceStack.Auth;
using Moq;
using System.Collections.Generic;

namespace ServiceStackAuth.Tests
{
    [TestFixture]
    public class UserManagementCommandServicesTests
    {
        private readonly ServiceStackHost appHost;

        public UserManagementCommandServicesTests()
        {
            appHost = new BasicAppHost(typeof(UserManagementCommandServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    container.Register<IUserAuthRepository>(GetMoq());
                }
            }
            .Init();
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void CanCreateUser()
        {
            var service = appHost.Container.Resolve<UserManagementCommandServices>();
            var response = (ResponseStatus)service.Any(new CreateUser { UserName = "Admin", Password = "admin", Email = "admin@mail.com", DisplayName="Administrator", Roles = new List<UserRole>() { new UserRole {  Id ="admin"    } } } );
            Assert.That(response.IsErrorResponse, Is.False);
        }


        [Test]
        public void CanUpdateUserWithoutChangingThePassword()
        {
            var service = appHost.Container.Resolve<UserManagementCommandServices>();
            var response = (ResponseStatus)service.Any(new UpdateUser { Id = "UserAuths-10", UserName = "Hamo", Email = "Hamo@mail.com", Roles = new List<UserRole>() { new UserRole { Id = "admin" } } });
            Assert.That(response.IsErrorResponse, Is.False);
        }

        [Test]
        public void CanUpdateUseWithPasswordChange()
        {
            var service = appHost.Container.Resolve<UserManagementCommandServices>();
            var response = (ResponseStatus)service.Any(new UpdateUser { Id = "UserAuths-10", UserName = "Hamo", Password = "Soso", Email = "soso@mail.com", Roles = new List<UserRole>() { new UserRole { Id = "admin" } } });
            Assert.That(response.IsErrorResponse, Is.False);
        }

        private IUserAuthRepository GetMoq()
        {
            var mock = new Mock<IUserAuthRepository>();
            
            mock.Setup(repository => repository.CreateUserAuth(It.IsAny<IUserAuth>(), It.IsAny<string>()))
                .Returns(
                    (IUserAuth u, string s) => 
                    {
                        return Create(u, s);
                    }
                );

            mock.Setup(repository => repository.GetUserAuth(It.IsAny<string>()))
                .Returns(new UserAuth() { Id = 10 });
            return mock.Object;
        }

        IUserAuth Create(IUserAuth ua, string password)
        {
            return ua;
        }
    }
}

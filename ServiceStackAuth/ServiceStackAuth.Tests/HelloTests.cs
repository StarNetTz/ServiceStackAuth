using System;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using ServiceStackAuth.ServiceModel;
using ServiceStackAuth.ServiceInterface;

namespace ServiceStackAuth.Tests
{
    [TestFixture]
    public class HelloTests
    {
        private readonly ServiceStackHost appHost;

        public HelloTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
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
        public void Test_Method1()
        {
            var kex = AesUtils.CreateKey().ToBase64UrlSafe();
            var service = appHost.Container.Resolve<MyServices>();

            var response = (HelloResponse)service.Any(new Hello { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }


    }
}

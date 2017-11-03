using NUnit.Framework;
using ServiceStackAuth.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackAuth.Tests
{
    [TestFixture]
    class DbIntegrationTests
    {
        [Test]
        public void can_create_roles_document()
        {
            RolesFactory.CreateRolesDocument(RavenGlobal.DocumentStore);
        }
    }
}

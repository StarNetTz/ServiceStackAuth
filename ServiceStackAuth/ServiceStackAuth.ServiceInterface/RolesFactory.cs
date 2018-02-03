using Raven.Client;
using Raven.Client.Document;
using ServiceStackAuth.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackAuth.ServiceInterface
{
    public class RolesFactory
    {

        public static void CreateRolesDocument(IDocumentStore store)
        {
            var doc = new Roles();
            doc.Id = "Roles-1";
            doc.UserRoles.AddRange(CreateDefaultRoles());
            using (var s = store.OpenSession())
            {
                s.Store(doc);
                s.SaveChanges();
            }
        }

        private static List<Role> CreateDefaultRoles()
        {
            var list = new List<Role>();
            list.Add(new Role { Id = "admin", Name = "Admin" });
            list.Add(new Role { Id = "user", Name = "User" });
            return list;
        }
    }

   
}

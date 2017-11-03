using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using ServiceStack.Auth;
using ServiceStackAuth.ServiceModel;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStackAuth.ServiceInterface
{
    public class Users_Smart_Search : AbstractMultiMapIndexCreationTask<Users_Smart_Search.Result>
    {
        public class Result
        {
            public string Id { get; set; }

            public string UserName { get; set; }

            public string DisplayName { get; set; }

            public string Email { get; set; }

            public string Content { get; set; }

            public List<string> Roles { get; set; }
        }

        public class Projection
        {
            public string Id { get; set; }

            public string UserName { get; set; }

            public string DisplayName { get; set; }

            public string Email { get; set; }

            public List<string> Roles { get; set; }

        }

        public Users_Smart_Search()
        {
            AddMap<UserAuth>(teams => from t in teams
                                  select new
                                  {
                                      t.Id,
                                      t.UserName,
                                      t.DisplayName,
                                      t.Email,
                                      Content = new[] {
                                          t.UserName,
                                          t.DisplayName,
                                          t.Email
                                      },
                                      t.Roles
                                  });
      


            // mark 'Content' field as analyzed which enables full text search operations
            Index(x => x.Content, FieldIndexing.Analyzed);
            Index(x => x.Roles, FieldIndexing.Analyzed);
        }
    }

    public static class Extensions
    {
        public static User ToUserViewModel(this Users_Smart_Search.Projection proj, Roles doc)
        {
            var u = new User { Id = proj.Id, DisplayName = proj.DisplayName, Email = proj.Email, Username = proj.UserName };
            foreach (var ur in doc.UserRoles)
                u.Roles.Add(new UserRole { Id = ur.Id, Name = ur.Name, IsMemberOf = proj.Roles.Contains(ur.Id) });
            u.IsAdmin = proj.Roles.Contains("admin");
            return u;

        }
    }
}

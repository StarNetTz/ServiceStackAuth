using Raven.Client;
using ServiceStack;
using ServiceStackAuth.ServiceModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ServiceStackAuth.ServiceInterface
{
    public class UserManagementQueryServices : QueryServiceBase
    {
        public object Any(FindUsers request)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            var result = Search(request);
            s.Stop();
            result.Duration = s.ElapsedMilliseconds;
            return result;
        }

            PaginatedResult<User> Search(FindUsers request)
            {
                PaginatedResult<User> result = new PaginatedResult<User>() { Data = new List<User>() };

                RavenQueryStatistics statsRef = new RavenQueryStatistics();


                var results = DocumentSession.Query<Users_Smart_Search.Result, Users_Smart_Search>()
                    .Statistics(out statsRef)
                    .Search(x => x.Content, $"{request.Qry}*", escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                    .Skip(request.CurrentPage * request.PageSize)
                    .Take(request.PageSize)
                    .ProjectFromIndexFieldsInto<Users_Smart_Search.Projection>();


                result.Data = ConvertResultsToUsersViewModel(results.ToList());
                result.TotalItems = statsRef.TotalResults;
                result.TotalPages = result.TotalItems / request.PageSize;
                if ((result.TotalItems % request.PageSize) > 0)
                    result.TotalPages += 1;
                result.PageSize = request.PageSize;
                result.CurrentPage = request.CurrentPage;

                if (CurrentPageIsOverflown(result))
                    return Search(new FindUsers() { Qry = request.Qry, CurrentPage = 0, PageSize = request.PageSize });

                return result;
            }

                static bool CurrentPageIsOverflown(PaginatedResult<User> result)
                {
                    return (result.Data.Count == 0) && (result.TotalPages > 0);
                }

            private List<User> ConvertResultsToUsersViewModel(List<Users_Smart_Search.Projection> inputList)
            {
                List<User> result = new List<User>();
                var roles = DocumentSession.Load<Roles>("Roles-1");
                foreach (var i in inputList)
                {
                    result.Add(i.ToUserViewModel(roles));
                }
                return result;
            }

        public object Any(FindRoles request)
        {
            return DocumentSession.Load<Roles>("Roles-1").UserRoles;
        }
    }

   
}
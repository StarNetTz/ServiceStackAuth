using System.Collections.Generic;
using ServiceStack;

namespace ServiceStackAuth.ServiceModel
{
    [Route("/users", Verbs ="POST")]
    public class CreateUser : IReturn<ResponseStatus>
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<UserRole> Roles { get; set; }

        public CreateUser()
        {
            Roles = new List<UserRole>();
        }
    }

    [Route("/users", Verbs = "PUT")]
    public class UpdateUser : IReturn<ResponseStatus>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<UserRole> Roles { get; set; }

        public UpdateUser()
        {
            Roles = new List<UserRole>();
        }
    }



    [Route("/users", Verbs = "GET")]
    public class FindUsers : IReturn<PaginatedResult<object>>
    {
        public string Qry { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public long Duration { get; set; }
    }

    [Route("/roles", Verbs = "GET")]
    public class FindRoles : IReturn<List<UserRole>>
    {
    }

}
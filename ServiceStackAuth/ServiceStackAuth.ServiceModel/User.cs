using System;
using System.Collections.Generic;

namespace ServiceStackAuth.ServiceModel
{

    public class User 
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public List<UserRole> Roles { get; set; }
        public bool IsAdmin { get; set; }
        public User()
        {
            Roles = new List<UserRole>();
        }
    }

    public class UserRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMemberOf { get; set; }
    }
}
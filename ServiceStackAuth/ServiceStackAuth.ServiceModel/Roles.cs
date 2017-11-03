using System;
using System.Collections.Generic;

namespace ServiceStackAuth.ServiceModel
{
    public class Roles
    {
        public string Id { get; set; }
        public List<Role> UserRoles { get; set; }

        public Roles()
        {
            UserRoles = new List<Role>();
        }
    }

    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
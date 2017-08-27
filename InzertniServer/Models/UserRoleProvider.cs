using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace InzertniServer.Models
{
    public class UserRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleType)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleType);
        }

        public override string[] GetRolesForUser(string username)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Login.Equals(username));
                if (user != null)
                {
                    string[] roles = (from x in db.Roles
                        join y in db.Users on x.RoleId equals y.Role.RoleId
                        where y.Login.Equals(username)
                        select x.RoleType).ToArray<string>();
                    return roles;
                }
                return new string[] { };
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}
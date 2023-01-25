using System.Linq;
using System.Web.Security;
using System.Web;
using System;
using deliverySystem.Models;

namespace deliverySystem.Authentication
{
    public class CustomRole : RoleProvider
    {

        public override bool IsUserInRole(string email, string roleName)
        {
            var userRoles = GetRolesForUser(email);
            return userRoles.Contains(roleName);
        }

        public override string[] GetRolesForUser(string email)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (OfficeDeliveryContext dbContext = new OfficeDeliveryContext())
            {
                var selectedUser = (from us in dbContext.Users
                                    where string.Compare(us.Email, email, StringComparison.OrdinalIgnoreCase) == 0
                                    select us).FirstOrDefault();
                if (selectedUser != null)
                {
                    userRoles = new[] { Enum.GetName(typeof(Role), selectedUser.Role) };
                }

                return userRoles.ToArray();
            }
        }

        #region Overrides of Role Provider

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
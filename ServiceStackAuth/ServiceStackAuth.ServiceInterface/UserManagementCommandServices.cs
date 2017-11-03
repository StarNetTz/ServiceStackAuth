using ServiceStack;
using ServiceStackAuth.ServiceModel;
using ServiceStack.Auth;
using System.Linq;

namespace ServiceStackAuth.ServiceInterface
{
    public class UserManagementCommandServices : Service
    {
        public IUserAuthRepository UserAuthRepository { get; set; }

        public object Any(CreateUser request)
        {
            var ua = new UserAuth() { UserName = request.UserName, Email = request.Email, DisplayName = request.DisplayName };
            ua.Roles = request.Roles.Where(r => r.IsMemberOf).Select(r => r.Id).ToList();
            var retval = UserAuthRepository.CreateUserAuth(ua, request.Password);
            return new ResponseStatus();
        }


        public object Any(UpdateUser request)
        {
            var original = UserAuthRepository.GetUserAuth(request.Id);
            var updated = UserAuthRepository.GetUserAuth(request.Id);
            ApplyUpdateToModel(request, updated);
            Update(request, original, updated);
            return new ResponseStatus();
        }

            private static void ApplyUpdateToModel(UpdateUser request, IUserAuth updated)
            {
                updated.UserName = request.UserName;
                updated.DisplayName = request.DisplayName;
                updated.Email = request.Email;
                updated.Roles = request.Roles.Where(r=>r.IsMemberOf).Select(r=>r.Id).ToList();
            }

            private void Update(UpdateUser request, IUserAuth original, IUserAuth updated)
            {
                if (!string.IsNullOrWhiteSpace(request.Password))
                    UserAuthRepository.UpdateUserAuth(original, updated, request.Password);
                else
                    UserAuthRepository.UpdateUserAuth(original, updated);
            }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication16.Models
{
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(UserManager<User, string> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            var userIdentity = base.CreateUserIdentityAsync(user);

            return userIdentity;
        }
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            var manager = new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
            return manager;
        }
        public virtual async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            
            if (false)
            {
                return SignInStatus.LockedOut;
            }

            var user = await UserManager.FindByNameAsync(userName).ConfigureAwait(false);

            if (user != null && await UserManager.CheckPasswordAsync(user, password).ConfigureAwait(false))
            {
                await SignInAsync(user, isPersistent, shouldLockout).ConfigureAwait(false);
                return SignInStatus.Success;
            }

            return SignInStatus.Failure;
        }
    }

}
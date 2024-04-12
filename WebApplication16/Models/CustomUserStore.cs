using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using IdentityResult = Microsoft.AspNet.Identity.IdentityResult;

namespace WebApplication16.Models
{
    public class CustomUserStore : Microsoft.AspNet.Identity.IUserStore<User>, Microsoft.AspNet.Identity.IUserPasswordStore<User>, Microsoft.AspNet.Identity.IUserEmailStore<User>
    {
        private readonly BookingEntities _dbContext;
        //private readonly UserLockoutService _userLockoutService;
        public CustomUserStore(BookingEntities dbContext)
        {
            _dbContext = dbContext;
            //_userLockoutService = userLockoutService;
        }
        public Task CreateAsync(User user)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);
            return _dbContext.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return _dbContext.Users.FindAsync(userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public void Dispose()
        {
            // Dispose resources if needed
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetEmailAsync(User user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Email = email;
            return UpdateAsync(user);
        }

        public Task<string> GetEmailAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return UpdateAsync(user);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            throw new NotImplementedException();
        }
      

    }
}
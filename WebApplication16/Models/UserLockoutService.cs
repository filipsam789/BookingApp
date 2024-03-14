using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication16.Models
{
    public class UserLockoutService
    {
        private readonly IMemoryCache _cache;

        public UserLockoutService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool GetLockoutEnabled(User user)
        {
            if (_cache.TryGetValue(GetLockoutKey(user.Id), out bool lockoutEnabled))
            {
                return lockoutEnabled;
            }

            return true; // Return true if the key is not found in the cache
        }

        public void SetLockoutEnabled(User user, bool enabled)
        {
            _cache.Set(GetLockoutKey(user.Id), enabled);
        }

        // Helper method to generate a unique key for lockout status
        private string GetLockoutKey(string userId) => $"UserLockout:{userId}";
    }
}
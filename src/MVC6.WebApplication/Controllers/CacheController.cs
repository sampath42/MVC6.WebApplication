using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MVC6.WebApplication.Models;

namespace MVC6.WebApplication.Controllers
{
    public class CacheController : Controller
    {
        private IMemoryCache cache;
        private const string GetOrCreate = "GetOrCreate";
        private const string CallbackMessage = "CallbackMessage";
        private const string CallbackEntry = "CallbackEntry";

        public CacheController(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Callback()
        {
            return View();
        }

        public IActionResult CacheTryGetValueSet()
        {
            DateTime cacheEntry;
            if(!cache.TryGetValue(GetOrCreate,out cacheEntry))
            {
                cacheEntry = DateTime.Now;
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(5));
                cache.Set(GetOrCreate, cacheEntry, cacheEntryOptions);
            }

            return View("Index", cacheEntry);
        }

        public IActionResult CacheGetOrCreate()
        {
            var cacheEntry = cache.GetOrCreate(GetOrCreate, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(10);
                return DateTime.Now;
            });

            return View("Index", cacheEntry);
        }

        public async Task<IActionResult> CacheGetOrCreateAsync()
        {
            var cacheEntry = await cache.GetOrCreateAsync(GetOrCreate, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(5);
                return Task.FromResult(DateTime.Now);
            });

            return View("Index", cacheEntry);
        }

        public IActionResult CacheGet()
        {
            var cacheEntry = cache.Get(GetOrCreate);
            return View("Index", cacheEntry);
        }

        public IActionResult CreateCallbackEntry()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromSeconds(5))
                .RegisterPostEvictionCallback(EvictionCallback, this);
            cache.Set(CallbackEntry, DateTime.Now, cacheEntryOptions);
            return RedirectToAction("GetCallbackEntry");
        }

        public IActionResult GetCallbackEntry()
        {
            return View("Callback", new CallbackViewModel
            {
                CachedTime = cache.Get<DateTime?>(CallbackEntry),
                Message = cache.Get<string>(CallbackMessage)
            });
        }

        public IActionResult RemoveCallbackEntry()
        {
            cache.Remove(CallbackEntry);
            return RedirectToAction("GetCallbackEntry");
        }

        private static void EvictionCallback(object key, object val, EvictionReason reason, object state)
        {
            var message = $"Entry was evicted. Reason: {reason}.";
            ((CacheController)state).cache.Set(CallbackMessage, message);
        }
        
    }
}
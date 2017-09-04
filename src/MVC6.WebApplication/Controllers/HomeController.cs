using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Cors;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace MVC6.WebApplication.Controllers
{
    [EnableCors("AllowOrigin1")]
    public class HomeController : Controller
    {
        private IMemoryCache cache;

        private string ClientId = "7b871190-3dae-459e-b777-ab4b6445411a";
        private string ClientSecret = "df3e8eaEc6zToSyeV3ZBE5yqjIurkc4dtwxpfwLifJk=";
        private string KeyVaultBaseUrl = "https://keyvaultslearning.vault.azure.net/keys/AzureKeyVaultTest/f2df95a5ba884bb8890bddb0269c2a67";
        private string KeyName = "AzureKeyVaultTest";

        public HomeController(IMemoryCache cache)
        {
            this.cache = cache;
        }

        [EnableCors("AllowOrigin1")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var keyVaultClient = new KeyVaultClient(AuthenticateVault);
            var result = await keyVaultClient.GetKeyAsync(KeyVaultBaseUrl);
            var configKeys = result.Tags;
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        private async Task<string> AuthenticateVault(string authority, string resource, string scope)
        {
            var clientCredential = new ClientCredential(ClientId,ClientSecret);
            var authenticationContext = new AuthenticationContext(authority);
            var result = await authenticationContext.AcquireTokenAsync(resource, clientCredential);
            return result.AccessToken;
        }     

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

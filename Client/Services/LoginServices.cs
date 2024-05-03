using System.Xml.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using Aon_PMS.Shared.Models;




namespace Aon_PMS.Services
{
    public class LoginServices
    {
        private LocalStorageAccessor localstorage;

        private HttpService httpService;

        public LoginServices(LocalStorageAccessor _localStorage, HttpService _httpService)
        {
            localstorage = _localStorage;

            httpService = _httpService;

        }

        public UsersM AuthUser { get; private set; }


        public async Task<UsersM> logUser()
        {
            var json = await localstorage.GetValueAsync("logUser");
            if (json != null)
            {
                AuthUser = JsonConvert.DeserializeObject<UsersM>(json);
                return AuthUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Login(UsersM log_user)
        {
            var json = await httpService.Post<UsersM>("UserMaster/Login", log_user);

            if(json != null)
            {
                await localstorage.SteValueAsync("logUser", json);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task Logout()
        {
            await localstorage.Clear();
            AuthUser = null;
        }

    }
}

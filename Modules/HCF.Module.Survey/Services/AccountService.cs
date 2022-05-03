using Blazored.LocalStorage;
using HCF.Module.Survey.Auth;
using HCF.Module.Survey.Model;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HCF.Module.Survey.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _customAuthenticationProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        public AccountService(ILocalStorageService localStorageService,
            AuthenticationStateProvider customAuthenticationProvider,
            HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _customAuthenticationProvider = customAuthenticationProvider;
            _httpClient = httpClient;
        }

        public User User { get; private set; }
       
        public async Task Initialize()
        {
            User = await _localStorageService.GetItemAsync<User>("user");
        }

        public async Task<User> GetCurrentUser()
        {
           var user = await _localStorageService.GetItemAsync<User>("user");
            return user;
        }

        public async Task<bool> LoginAsync(LoginModel model)
        {
            //var response = await _httpClient.PostAsJsonAsync<LoginModel>("/account/login-token", model);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return false;
            //}
            //AuthResponse authData = await response.Content.ReadFromJsonAsync<AuthResponse>();

            var user = new User
            {
                Id = 123, FirstName="Anoop Logged In"
            };
            User = user;
            await _localStorageService.SetItemAsync("user", user);
            await _localStorageService.SetItemAsync("token", model.code);
            await _localStorageService.SetItemAsync("refreshToken", model.code);
            // (_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("user");
            await _localStorageService.RemoveItemAsync("token");
            //(_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
            return true;
        }
    }
}

using DocumentController.WPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentController.WPF.Services
{
    public class CloudAdminUserService : IAdminUserService
    {
        private HttpClient client;
        public CloudAdminUserService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri((Application.Current as App).ApiEndPointBaseAddress)
            };
        }

        public async Task<bool> IsAdmin(string username)
        {
            var adminUsers = new List<AdminUser>();

            var response = await client.GetAsync("adminUsers");
            if (response.IsSuccessStatusCode)
            {
                adminUsers.AddRange(JsonConvert.DeserializeObject<List<AdminUser>>(await response.Content.ReadAsStringAsync()));
            }

            var usernames = adminUsers.Select(au => au.Username.ToLower()).ToList();

            if (usernames.Contains(username.ToLower()))
                return true;

            return false;
        }
    }
}

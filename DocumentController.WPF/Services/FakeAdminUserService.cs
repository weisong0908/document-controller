using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Services
{
    public class FakeAdminUserService : IAdminUserService
    {
        public Task<bool> IsAdmin(string username)
        {
            return Task.Run(() => true);
        }
    }
}

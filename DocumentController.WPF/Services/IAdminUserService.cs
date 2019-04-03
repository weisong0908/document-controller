using System.Threading.Tasks;

namespace DocumentController.WPF.Services
{
    public interface IAdminUserService
    {
        Task<bool> IsAdmin(string username);
    }
}
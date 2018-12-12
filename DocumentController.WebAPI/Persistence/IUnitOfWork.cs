using System.Threading.Tasks;

namespace DocumentController.WebAPI.Persistence
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
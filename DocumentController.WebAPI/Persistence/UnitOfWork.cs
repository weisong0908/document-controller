using System.Threading.Tasks;

namespace DocumentController.WebAPI.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DocumentControllerDbContext dbContext;

        public UnitOfWork(DocumentControllerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CompleteAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
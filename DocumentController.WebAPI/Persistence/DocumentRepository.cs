using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentController.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentController.WebAPI.Persistence
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentControllerDbContext dbContext;
        public DocumentRepository(DocumentControllerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Document>> GetAllDocuments()
        {
            return await dbContext.Documents.ToListAsync();
        }

        public async Task<Document> GetDocument(int id)
        {
            return await dbContext.Documents.SingleOrDefaultAsync(d => d.Id == id);
        }
    }
}
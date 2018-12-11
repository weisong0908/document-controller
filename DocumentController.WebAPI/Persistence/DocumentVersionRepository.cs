using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentController.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentController.WebAPI.Persistence
{
    public class DocumentVersionRepository : IDocumentVersionRepository
    {
        private readonly DocumentControllerDbContext dbContext;
        public DocumentVersionRepository(DocumentControllerDbContext context)
        {
            this.dbContext = context;
        }
        public async Task<IEnumerable<DocumentVersion>> GetAllDocumentVersionsByDocumentId(int documentId)
        {
            return await dbContext.DocumentVersions.Where(dv => dv.DocumentId == documentId).Where(dv => dv.IsRemoved != "true").ToListAsync();
        }

        public Task<DocumentVersion> GetDocumentVersion(int id)
        {
            return dbContext.DocumentVersions.Where(dv => dv.IsRemoved != "true").SingleOrDefaultAsync(dv => dv.Id == id);
        }
    }
}
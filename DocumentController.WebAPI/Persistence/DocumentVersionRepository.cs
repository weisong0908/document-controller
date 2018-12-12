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

        public async Task AddNewDocumentVersion(DocumentVersion documentVersion)
        {
            await dbContext.DocumentVersions.AddAsync(documentVersion);
        }

        public async Task<DocumentVersion> UpdateDocumentVersion(DocumentVersion documentVersion)
        {
            var documentVersionInDb = await dbContext.DocumentVersions.Where(dv => dv.IsRemoved != "true").SingleOrDefaultAsync(dv => dv.Id == documentVersion.Id);

            if (documentVersionInDb == null)
                return null;
            dbContext.Entry(documentVersionInDb).State = EntityState.Detached;

            dbContext.DocumentVersions.Update(documentVersion);

            return documentVersion;
        }

        public async Task<DocumentVersion> RemoveDocumentVersion(int documentVersionId)
        {
            var documentVersionInDb = await dbContext.DocumentVersions.SingleOrDefaultAsync(dv => dv.Id == documentVersionId);

            if (documentVersionInDb == null)
                return null;
            documentVersionInDb.IsRemoved = "true";

            dbContext.DocumentVersions.Update(documentVersionInDb);

            return documentVersionInDb;
        }
    }
}
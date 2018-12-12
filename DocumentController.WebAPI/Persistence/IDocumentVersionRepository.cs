using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentController.WebAPI.Models;

namespace DocumentController.WebAPI.Persistence
{
    public interface IDocumentVersionRepository
    {
        Task<IEnumerable<DocumentVersion>> GetAllDocumentVersionsByDocumentId(int documentId);
        Task<DocumentVersion> GetDocumentVersion(int id);
        Task AddNewDocumentVersion(DocumentVersion documentVersion);
        Task<DocumentVersion> UpdateDocumentVersion(DocumentVersion documentVersion);
        Task<DocumentVersion> RemoveDocumentVersion(int documentVersionId);
    }
}
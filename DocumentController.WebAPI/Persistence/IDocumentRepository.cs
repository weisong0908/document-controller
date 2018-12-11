using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentController.WebAPI.Models;

namespace DocumentController.WebAPI.Persistence
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllDocuments();
        Task<Document> GetDocument(int id);
    }
}
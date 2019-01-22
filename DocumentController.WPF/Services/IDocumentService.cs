using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentController.WPF.Models;

namespace DocumentController.WPF.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetDocuments();

        Task<Document> AddNewDocument(Document document);
    }
}
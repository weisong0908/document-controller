using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentController.WPF.Models;

namespace DocumentController.WPF.Services
{
    public interface IDocumentVersionService
    {
        Task<IEnumerable<DocumentVersion>> GetAllVersionsByDocumentId(int documentId);
    }
}
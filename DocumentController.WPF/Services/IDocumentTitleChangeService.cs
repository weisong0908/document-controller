using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentController.WPF.Models;

namespace DocumentController.WPF.Services
{
    public interface IDocumentTitleChangeService
    {
        Task<DocumentTitleChange> AddNewDocumentTitleChange(DocumentTitleChange documentTitleChange);
        Task<IEnumerable<DocumentTitleChange>> GetDocumentTitleChangeByDocumentId(int documentId);
    }
}
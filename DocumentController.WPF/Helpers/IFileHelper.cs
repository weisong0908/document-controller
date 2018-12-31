using DocumentController.WPF.Models;
using DocumentController.WPF.ViewModels;

namespace DocumentController.WPF.Helpers
{
    public interface IFileHelper
    {
        string GetDocumentLocation(DocumentViewModel document);

        void GoToFile(DocumentViewModel document);

        void UpdateFiles(DocumentViewModel document, DocumentVersionViewModel documentVersion);

        string GetFilePath(FileType fileType);
    }
}

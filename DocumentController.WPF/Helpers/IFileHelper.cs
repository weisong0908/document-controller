using DocumentController.WPF.Models;
using DocumentController.WPF.ViewModels;
using DocumentController.WPF.Helpers;

namespace DocumentController.WPF.Helpers
{
    public interface IFileHelper
    {
        string GetDocumentLocation(DocumentViewModel document);

        void GoToFile(DocumentViewModel document);

        void UpdateFiles(DocumentViewModel document, DocumentVersionViewModel documentVersion = null, UpdateFilesMethod updateFilesMethod = UpdateFilesMethod.UpdateVersion);

        void UpdateFiles(DocumentViewModel originalDocument, DocumentViewModel newDocument, DocumentVersionViewModel documentVersion = null, UpdateFilesMethod updateFilesMethod = UpdateFilesMethod.UpdateVersion);

        string GetFilePath(FileType fileType);

        void BackUpDatabase();
    }
}

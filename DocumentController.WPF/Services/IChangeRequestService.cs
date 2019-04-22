using DocumentController.WPF.Models;
using DocumentController.WPF.ViewModels;

namespace DocumentController.WPF.Services
{
    public interface IDocumentChangeRequestService
    {
        DocumentChangeRequest ReadDcrForm(string dcrFormPath);

        void WriteDcrForm(string dcrFormTemplatePath, string dcrFormDestinationPath, DocumentViewModel currentDocument);
    }
}
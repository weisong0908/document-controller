using DocumentController.WPF.Models;

namespace DocumentController.WPF.Services
{
    public interface IDocumentChangeRequestService
    {
        DocumentChangeRequest ReadDcrForm(string dcrFormPath);
    }
}
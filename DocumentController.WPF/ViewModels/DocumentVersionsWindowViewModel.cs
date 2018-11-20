using AutoMapper;
using DocumentController.WPF.Helpers;
using DocumentController.WPF.Models;
using DocumentController.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.ViewModels
{
    public class DocumentVersionsWindowViewModel : BaseViewModel
    {
        private readonly IDocumentVersionService documentVersionService;
        private IMapper mapper;

        public IEnumerable<string> Progresses { get; set; }
        private DocumentViewModel _selectedDocument;
        public DocumentViewModel SelectedDocument
        {
            get { return _selectedDocument; }
            set { SetValue(ref _selectedDocument, value); }
        }
        private ObservableCollection<DocumentVersionViewModel> _documentVersions;
        public ObservableCollection<DocumentVersionViewModel> DocumentVersions
        {
            get { return _documentVersions; }
            set { SetValue(ref _documentVersions, value); }
        }
        private DocumentVersionViewModel _selectedDocumentVersion;

        public DocumentVersionViewModel SelectedDocumentVersion
        {
            get { return _selectedDocumentVersion; }
            set { SetValue(ref _selectedDocumentVersion, value); }
        }

        public DocumentVersionsWindowViewModel(IDocumentVersionService documentVersionService)
        {
            this.documentVersionService = documentVersionService;
            mapper = Mapper.Instance;

            Progresses = typeof(Progress).GetFields().Select(f => f.GetValue(null).ToString());
            DocumentVersions = new ObservableCollection<DocumentVersionViewModel>();
        }

        public async void OnStartUp(DocumentViewModel selectedDocument)
        {
            SelectedDocument = selectedDocument;
            var documentVersions = mapper.Map<List<DocumentVersionViewModel>>((await documentVersionService.GetAllVersionsByDocumentId(_selectedDocument.Id)).OrderByDescending(dv => dv.EffectiveDate));
            DocumentVersions = new ObservableCollection<DocumentVersionViewModel>(documentVersions);

            SelectedDocumentVersion = _documentVersions
                .Where(dv => dv.Progress == Progress.InEffect)
                .OrderByDescending(dv => dv.EffectiveDate)
                .FirstOrDefault();
        }

        public async void SaveDocumentVersion()
        {
            if (SelectedDocumentVersion == null)
                return;

            if (ValidateDocumentVersionInput())
            {
                var originalDocumentVersions = DocumentVersions;

                var documentVersionForChange = mapper.Map<DocumentVersion>(_selectedDocumentVersion);
                object response;

                if (documentVersionForChange.Id == 0)
                {
                    DocumentVersions.Add(SelectedDocumentVersion);
                    DocumentVersions = new ObservableCollection<DocumentVersionViewModel>(DocumentVersions.OrderByDescending(dv => dv.EffectiveDate));

                    response = await documentVersionService.AddNewDocumentVersion(documentVersionForChange);
                    if (response == null)
                    {
                        WindowHelper.Alert("Please update again", "Opps, something went wrong");
                        DocumentVersions = originalDocumentVersions;
                    }
                }
                else
                {
                    response = await documentVersionService.UpdateDocumentVersion(documentVersionForChange);
                    if (response == null)
                        WindowHelper.Alert("Please update again", "Opps, something went wrong");
                }

                var documentVersions = mapper.Map<List<DocumentVersionViewModel>>((await documentVersionService.GetAllVersionsByDocumentId(_selectedDocument.Id)).OrderByDescending(dv => dv.EffectiveDate));
            }
        }

        public void CreateNewDocumentVersion()
        {
            SelectedDocumentVersion = new DocumentVersionViewModel(_selectedDocument.Id);
        }

        public void RemoveDocumentVersion()
        {
            var originalDocumentVersions = DocumentVersions;

            documentVersionService.RemoveDocumentVersion(mapper.Map<DocumentVersion>(_selectedDocumentVersion));
            DocumentVersions.Remove(_selectedDocumentVersion);
        }

        public void UploadDocument()
        {

        }

        public void OnLocatePdf()
        {

        }

        public void OnLocateEditable()
        {

        }

        private bool ValidateDocumentVersionInput()
        {
            if (string.IsNullOrWhiteSpace(_selectedDocumentVersion.VersionNumber))
            {
                WindowHelper.Alert("The version number cannot be empty", "Invalid input");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_selectedDocumentVersion.Progress))
            {
                WindowHelper.Alert("The progress cannot be empty", "Invalid input");
                return false;
            }

            if (_selectedDocumentVersion.EffectiveDate == null)
            {
                WindowHelper.Alert("The effective date cannot be empty. Put a tentative effective date if unsure.", "Invalid input");
                return false;
            }

            return true;
        }
    }
}

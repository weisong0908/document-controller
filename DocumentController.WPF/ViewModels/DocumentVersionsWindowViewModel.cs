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
        private readonly IAdminUserService adminUserService;
        private readonly IDocumentChangeRequestService changeRequestService;
        private readonly IFileHelper fileHelper;
        private readonly IWindowHelper windowHelper;
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

        private string _newDocumentName;
        public string NewDocumentTitle
        {
            get { return _newDocumentName; }
            set { SetValue(ref _newDocumentName, value); }
        }

        public bool IsAdmin { get; set; }

        public string Visibility { get { return (IsAdmin) ? "Visible" : "Collapsed"; } }
        public string IsReadOnly { get { return (IsAdmin) ? "False" : "True"; } }
        public string IsEnabled { get { return (IsAdmin) ? "True" : "False"; } }

        public DocumentVersionsWindowViewModel(IDocumentVersionService documentVersionService, IAdminUserService adminUserService, IDocumentChangeRequestService changeRequestService, IFileHelper fileHelper, IWindowHelper windowHelper, IMapper mapper)
        {
            this.documentVersionService = documentVersionService;
            this.adminUserService = adminUserService;
            this.changeRequestService = changeRequestService;
            this.fileHelper = fileHelper;
            this.windowHelper = windowHelper;
            this.mapper = mapper;

            Progresses = typeof(Progress).GetFields().Select(f => f.GetValue(null).ToString());
            DocumentVersions = new ObservableCollection<DocumentVersionViewModel>();
        }

        public async void OnStartUp(DocumentViewModel selectedDocument)
        {
            IsAdmin = await adminUserService.IsAdmin(Environment.UserName);
            OnPropertyChanged(nameof(Visibility));
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(IsEnabled));

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
                        windowHelper.Alert("Please update again", "Opps, something went wrong");
                        DocumentVersions = originalDocumentVersions;
                    }
                }
                else
                {
                    response = await documentVersionService.UpdateDocumentVersion(documentVersionForChange);
                    if (response == null)
                        windowHelper.Alert("Please update again", "Opps, something went wrong");
                }

                SelectedDocumentVersion.Id = (response as DocumentVersion).Id;

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
            fileHelper.UpdateFiles(_selectedDocument, _selectedDocumentVersion, UpdateFilesMethod.UpdateVersion);
        }

        public void BrowsePDFFile()
        {
            var filePath = fileHelper.GetFilePath(FileType.PDF);

            if (string.IsNullOrEmpty(filePath))
                return;

            SelectedDocumentVersion.Location_PDF = filePath;
        }

        public void BrowseEditableFile()
        {
            var filePath = fileHelper.GetFilePath(FileType.Editable);

            if (string.IsNullOrEmpty(filePath))
                return;

            SelectedDocumentVersion.Location_Editable = filePath;
        }

        public void OnCreateNewDocumentChangeRequested()
        {
            var filename = fileHelper.SaveFile($"DCR - {_selectedDocument.Title} - {DateTime.Today.ToString("yyyy-MM-dd")}");

            changeRequestService.WriteDcrForm(@"C:\Users\weisong.teng\Desktop\Document Change Request - V4.pdf", filename, SelectedDocument);
        }

        public void OnImportDocumentChangeRequest()
        {
            var dcrFormPath = fileHelper.GetFilePath(FileType.PDF);
            var documentChangeRequest = changeRequestService.ReadDcrForm(dcrFormPath);
            if (documentChangeRequest == null)
                return;

            CreateNewDocumentVersion();

            SelectedDocumentVersion.DescriptionOfChange = documentChangeRequest.DescriptionOfChange;
            var effectiveDate = new DateTime();
            if (DateTime.TryParse(documentChangeRequest.RevisedEffectiveDate, out effectiveDate))
                SelectedDocumentVersion.EffectiveDate = effectiveDate;
            SelectedDocumentVersion.PurposeOfChange = documentChangeRequest.PurposeOfChange;
            SelectedDocumentVersion.Requestor = documentChangeRequest.RequestorName;
            SelectedDocumentVersion.VersionNumber = documentChangeRequest.RevisedVersionNumber;
            SelectedDocumentVersion.Remarks = documentChangeRequest.Remarks;
        }

        private bool ValidateDocumentVersionInput()
        {
            if (string.IsNullOrWhiteSpace(_selectedDocumentVersion.VersionNumber))
            {
                windowHelper.Alert("The version number cannot be empty", "Invalid input");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_selectedDocumentVersion.Progress))
            {
                windowHelper.Alert("The progress cannot be empty", "Invalid input");
                return false;
            }

            if (_selectedDocumentVersion.EffectiveDate == null)
            {
                windowHelper.Alert("The effective date cannot be empty. Put a tentative effective date if unsure.", "Invalid input");
                return false;
            }

            return true;
        }
    }
}

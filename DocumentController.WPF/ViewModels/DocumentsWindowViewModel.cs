using AutoMapper;
using DocumentController.WPF.Helpers;
using DocumentController.WPF.Models;
using DocumentController.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.ViewModels
{
    public class DocumentsWindowViewModel : BaseViewModel
    {
        private readonly IDocumentService documentService;
        private readonly IDocumentVersionService documentVersionService;
        private readonly IMapper mapper;

        private IList<DocumentViewModel> _allDocuments;
        private IList<DocumentViewModel> _filteredDocuments;
        public IList<DocumentViewModel> FilteredDocuments
        {
            get { return _filteredDocuments; }
            set { SetValue(ref _filteredDocuments, value); }
        }
        private DocumentViewModel _selectedDocument;
        public DocumentViewModel SelectedDocument
        {
            get { return _selectedDocument; }
            set { SetValue(ref _selectedDocument, value); }
        }

        public DocumentsWindowViewModel(IDocumentService documentService, IDocumentVersionService documentVersionService)
        {
            this.documentService = documentService;
            this.documentVersionService = documentVersionService;
            mapper = Mapper.Instance;

            FilteredDocuments = new ObservableCollection<DocumentViewModel>();

            OnActivated();
        }

        public async void OnActivated()
        {
            _allDocuments = mapper.Map<IList<DocumentViewModel>>(await documentService.GetDocuments());
            if (_allDocuments == null)
                return;

            FilteredDocuments = _allDocuments;
        }

        public void FilterDocuments(string searchText)
        {
            var results = _allDocuments
                .Where(d => d.Title.ToLower().Contains(searchText.ToLower()) || d.DocumentNumber.ToLower().Contains(searchText.ToLower()))
                .ToList();

            if (results == null)
                return;
            FilteredDocuments = results;
        }

        public async void SelectDocument(DocumentViewModel selectedDocument)
        {
            SelectedDocument = selectedDocument;

            var allDocumentVersions = mapper.Map<IList<DocumentVersionViewModel>>(await documentVersionService.GetAllVersionsByDocumentId(selectedDocument.Id));
            var latestDocumentVersion = allDocumentVersions.Where(dv => dv.Progress == Progress.InEffect).OrderByDescending(dv => dv.EffectiveDate).FirstOrDefault();

            if (latestDocumentVersion == null)
                return;

            SelectedDocument.VersionNumber = latestDocumentVersion.VersionNumber;
            SelectedDocument.EffectiveDate = latestDocumentVersion.EffectiveDate;
            SelectedDocument.Location = FileHelper.GetDocumentLocation(_selectedDocument);
        }

        public void GoToVersionsWindow()
        {
            if (_selectedDocument == null)
                return;

            WindowHelper.ShowWindow(WindowHelper.WindowType.DocumentVersionWindow, _selectedDocument);
        }

        public void OnNewDocument()
        {

        }

        public void OnNavigateToDocumentLocation()
        {
            FileHelper.GoToFile(_selectedDocument);
        }

        public void OnBackUpDatabase()
        {

        }
    }
}

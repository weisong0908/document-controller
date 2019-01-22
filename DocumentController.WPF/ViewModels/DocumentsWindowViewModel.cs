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
        private readonly IFileHelper fileHelper;
        private readonly IWindowHelper windowHelper;
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
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetValue(ref _searchText, value);
                FilterDocuments();
            }
        }

        public DocumentsWindowViewModel(IDocumentService documentService, IDocumentVersionService documentVersionService, IFileHelper fileHelper, IWindowHelper windowHelper, IMapper mapper)
        {
            this.documentService = documentService;
            this.documentVersionService = documentVersionService;
            this.fileHelper = fileHelper;
            this.windowHelper = windowHelper;
            this.mapper = mapper;

            FilteredDocuments = new ObservableCollection<DocumentViewModel>();

            OnActivated();
        }

        public async void OnActivated()
        {
            _allDocuments = mapper.Map<IList<DocumentViewModel>>(await documentService.GetDocuments());
            if (_allDocuments == null)
                return;

            if (string.IsNullOrEmpty(_searchText))
                FilteredDocuments = new ObservableCollection<DocumentViewModel>(_allDocuments);
        }

        public void FilterDocuments()
        {
            var results = _allDocuments
                .Where(d => d.Title.ToLower().Contains(_searchText.ToLower()) || d.DocumentNumber.ToLower().Contains(_searchText.ToLower()))
                .ToList();

            FilteredDocuments = new ObservableCollection<DocumentViewModel>(results);
        }

        public async void SelectDocument(DocumentViewModel selectedDocument)
        {
            SelectedDocument = selectedDocument;

            var allDocumentVersions = mapper.Map<IList<DocumentVersionViewModel>>(await documentVersionService.GetAllVersionsByDocumentId(selectedDocument.Id));
            var latestDocumentVersion = allDocumentVersions.Where(dv => dv.Progress == Progress.InEffect && dv.IsRemoved.ToLower() != "true").OrderByDescending(dv => dv.EffectiveDate).FirstOrDefault();

            if (latestDocumentVersion == null)
                return;

            SelectedDocument.VersionNumber = latestDocumentVersion.VersionNumber;
            SelectedDocument.EffectiveDate = latestDocumentVersion.EffectiveDate;
            SelectedDocument.Location = fileHelper.GetDocumentLocation(_selectedDocument);
        }

        public void GoToVersionsWindow()
        {
            if (_selectedDocument == null)
                return;

            windowHelper.ShowWindow(WindowType.DocumentVersionWindow, _selectedDocument);
        }

        public void OnNewDocument()
        {
            windowHelper.ShowWindow(WindowType.NewDocumentWindow);
        }

        public void OnRescindDocument()
        {
            if (_selectedDocument == null)
                return;

            if(windowHelper.Confirmation($"Are you sure you want to rescind {_selectedDocument.Title}?", "Rescind document"))
            {
                documentService.RemoveDocument(mapper.Map<Document>(_selectedDocument));
                FilteredDocuments.Remove(_selectedDocument);
                fileHelper.UpdateFiles(_selectedDocument, updateFilesMethod: UpdateFilesMethod.RescindDocument);
            }
        }

        public void OnNavigateToDocumentLocation()
        {
            fileHelper.GoToFile(_selectedDocument);
        }

        public void OnBackUpDatabase()
        {

        }
    }
}

﻿using AutoMapper;
using DocumentController.WPF.Helpers;
using DocumentController.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IList<DocumentVersionViewModel> _documentVersions;
        public IList<DocumentVersionViewModel> DocumentVersions
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

            Progresses = typeof(Models.Progress).GetFields().Select(f => f.GetValue(null).ToString());
            DocumentVersions = new ObservableCollection<DocumentVersionViewModel>();
        }

        public async void OnStartUp(DocumentViewModel selectedDocument)
        {
            SelectedDocument = selectedDocument;
            DocumentVersions = mapper.Map<List<DocumentVersionViewModel>>((await documentVersionService.GetAllVersionsByDocumentId(_selectedDocument.Id)).OrderByDescending(dv => dv.EffectiveDate).ToList());

            SelectedDocumentVersion = _documentVersions
                .Where(dv => dv.Progress == Models.Progress.InEffect)
                .OrderByDescending(dv => dv.EffectiveDate)
                .FirstOrDefault();
        }

        public void OnSaveVersion()
        {

        }

        public void OnNewVersion()
        {

        }

        public void OnRemoveVersion()
        {

        }

        public void UploadDocument()
        {
            FileHelper.UpdateFiles(_selectedDocument, _selectedDocumentVersion);
        }

        public void BrowsePDFFile()
        {
            var filePath = FileHelper.GetFilePath(FileHelper.FileType.PDF);

            if (string.IsNullOrEmpty(filePath))
                return;

            SelectedDocumentVersion.Location_PDF = filePath;
        }

        public void BrowseEditableFile()
        {
            var filePath = FileHelper.GetFilePath(FileHelper.FileType.Editable);

            if (string.IsNullOrEmpty(filePath))
                return;

            SelectedDocumentVersion.Location_Editable = filePath;
        }
    }
}

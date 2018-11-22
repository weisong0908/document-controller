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

        private DocumentViewModel _selectedDocument;
        public DocumentViewModel SelectedDocument { get { return _selectedDocument; } set { _selectedDocument = value; } }
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

            DocumentVersions = new ObservableCollection<DocumentVersionViewModel>();
        }

        public async void OnStartUp(DocumentViewModel selectedDocument)
        {
            SelectedDocument = selectedDocument;
            DocumentVersions = mapper.Map<List<DocumentVersionViewModel>>((await documentVersionService.GetAllVersionsByDocumentId(_selectedDocument.Id)).OrderByDescending(dv => dv.EffectiveDate).ToList());
        }

        public void OnVersionSelected(DocumentVersionViewModel documentVersion)
        {
            SelectedDocumentVersion = documentVersion;
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

        public void OnLocatePdf()
        {

        }

        public void OnLocateEditable()
        {

        }
    }
}

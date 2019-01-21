using AutoMapper;
using DocumentController.WPF.Helpers;
using DocumentController.WPF.Models;
using DocumentController.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.ViewModels
{
    public class NewDocumentWindowViewModel : BaseViewModel
    {
        private readonly IDocumentService documentService;
        private readonly IWindowHelper windowHelper;
        private readonly IMapper mapper;

        public IEnumerable<string> DocumentTypes { get; set; }

        private DocumentViewModel _document;
        public DocumentViewModel Document
        {
            get { return _document; }
            set { SetValue(ref _document, value); }
        }

        public NewDocumentWindowViewModel(IDocumentService documentService, IWindowHelper windowHelper, IMapper mapper)
        {
            this.documentService = documentService;
            this.windowHelper = windowHelper;
            this.mapper = mapper;

            DocumentTypes = typeof(DocumentType).GetFields().Select(f => f.GetValue(null).ToString());

            Document = new DocumentViewModel();
        }

        public void CreateNewDocument()
        {
            _document.Id = 571;

            windowHelper.CloseWindow();
            windowHelper.ShowWindow(WindowType.DocumentVersionWindow, _document);
        }

        public void CancelNewDocument()
        {
            windowHelper.CloseWindow();
        }
    }
}

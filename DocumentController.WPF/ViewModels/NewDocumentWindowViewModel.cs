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
    public class NewDocumentWindowViewModel : BaseViewModel
    {
        private readonly IDocumentService documentService;
        private readonly IWindowHelper windowHelper;
        private readonly IMapper mapper;

        public IEnumerable<string> DocumentTypes { get; }
        public IEnumerable<string> Departments { get; }
        private ObservableCollection<string> _functions;
        public ObservableCollection<string> Functions
        {
            get { return _functions; }
            set { SetValue(ref _functions, value); }
        }

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
            Departments = typeof(Department).GetFields().Select(f => f.GetValue(null).ToString());

            Document = new DocumentViewModel();
        }

        public void OnDepartmentChanged()
        {
            if (_document == null)
                return;

            if (string.IsNullOrEmpty(_document.Department))
                return;

            Functions = new ObservableCollection<string>();
            switch (_document.Department)
            {
                case Department.Academic:
                    Functions.Add(Function.AcademicServices);
                    Functions.Add(Function.Examination);
                    Functions.Add(Function.AcademicSupport);
                    break;
                default:
                    Functions.Add(string.Empty);
                    break;
            }
        }

        public void CreateNewDocument()
        {
            _document.Id = 571;
            _document.Status = DocumentStatus.Active;

            windowHelper.CloseWindow();
            windowHelper.ShowWindow(WindowType.DocumentVersionWindow, _document);
        }

        public void CancelNewDocument()
        {
            windowHelper.CloseWindow();
        }
    }
}

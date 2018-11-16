using DocumentController.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.ViewModels
{
    public class DocumentViewModel : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetValue(ref _id, value); }
        }

        private string _documentNumber = string.Empty;
        public string DocumentNumber
        {
            get { return _documentNumber; }
            set { SetValue(ref _documentNumber, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        private string _department = string.Empty;
        public string Department
        {
            get { return _department; }
            set { SetValue(ref _department, value); }
        }

        private string _function = string.Empty;
        public string Function
        {
            get { return _function; }
            set { SetValue(ref _function, value); }
        }

        private string _type = string.Empty;
        public string Type
        {
            get { return _type; }
            set { SetValue(ref _type, value); }
        }

        private string _status = string.Empty;
        public string Status
        {
            get { return _status; }
            set { SetValue(ref _status, value); }
        }

        private string _location = string.Empty;
        public string Location
        {
            get { return _location; }
            set { SetValue(ref _location, value); }
        }

        private string _versionNumber;
        public string VersionNumber
        {
            get { return _versionNumber; }
            set { SetValue(ref _versionNumber, value); }
        }

        private DateTime? _effectiveDate;
        public DateTime? EffectiveDate
        {
            get { return _effectiveDate; }
            set { SetValue(ref _effectiveDate, value); }
        }

        public DocumentViewModel()
        {
            _documentNumber = string.Empty;
            _title = string.Empty;
            _department = string.Empty;
            _function = string.Empty;
            _type = string.Empty;
            _status = string.Empty;
            _location = string.Empty;
            _versionNumber = string.Empty;
            _effectiveDate = null;
        }

        private string GetDocumentLocation()
        {
            string sharedDrive = @"\\csing.navitas.local\shared\Documents\";
            string mainFolder = "";
            switch (Type)
            {
                case DocumentType.Policy:
                    mainFolder = @"# Curtin Singapore Corporate Policies #\" + Department;
                    break;
                case DocumentType.Procedure:
                    mainFolder = @"# Curtin Singapore Corporate Procedures #" + Department;
                    break;
                case DocumentType.Form:
                    mainFolder = @"= Controlled Document =\Forms & Templates\" + Department + @"\Editable";
                    break;
                case DocumentType.WorkInstruction:
                    mainFolder = @"= Controlled Document =\Work Instructions & Guidelines\" + Department;
                    break;
                case DocumentType.OrganisationChart:
                    mainFolder = @"= Controlled Document =\Organisation Chart";
                    break;
            }

            string folderPath = System.IO.Path.Combine(sharedDrive, mainFolder);
            string fullPath = System.IO.Path.Combine(folderPath, $"{Title} - V{VersionNumber}");

            return fullPath;
        }
    }
}

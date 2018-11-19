using DocumentController.WPF.Helpers;
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

        private string _documentNumber;
        public string DocumentNumber
        {
            get { return _documentNumber; }
            set { SetValue(ref _documentNumber, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        private string _department;
        public string Department
        {
            get { return _department; }
            set { SetValue(ref _department, value); }
        }

        private string _function;
        public string Function
        {
            get { return _function; }
            set { SetValue(ref _function, value); }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { SetValue(ref _type, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetValue(ref _status, value); }
        }

        private string _location;
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
    }
}

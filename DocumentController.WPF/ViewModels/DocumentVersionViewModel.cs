using DocumentController.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.ViewModels
{
    public class DocumentVersionViewModel : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetValue(ref _id, value, nameof(Id)); }
        }

        private int _documentId;
        public int DocumentId
        {
            get { return _documentId; }
            set { SetValue(ref _documentId, value, nameof(DocumentId)); ; }
        }

        private string _versionNumber;
        public string VersionNumber
        {
            get { return _versionNumber; }
            set { SetValue(ref _versionNumber, value, nameof(VersionNumber)); }
        }

        private DateTime? _effectiveDate;
        public DateTime? EffectiveDate
        {
            get { return _effectiveDate; }
            set { SetValue(ref _effectiveDate, value, nameof(EffectiveDate)); }
        }

        private string _progress;
        public string Progress
        {
            get { return _progress; }
            set { SetValue(ref _progress, value, nameof(Progress)); }
        }

        private string _descriptionOfChange;
        public string DescriptionOfChange
        {
            get { return _descriptionOfChange; }
            set { SetValue(ref _descriptionOfChange, value, nameof(DescriptionOfChange)); }
        }

        private string _purposeOfChange;
        public string PurposeOfChange
        {
            get { return _purposeOfChange; }
            set { SetValue(ref _purposeOfChange, value, nameof(PurposeOfChange)); }
        }

        private string _requestor;
        public string Requestor
        {
            get { return _requestor; }
            set { SetValue(ref _requestor, value, nameof(Requestor)); }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set { SetValue(ref _remarks, value, nameof(Remarks)); }
        }

        private string _location_PDF;
        public string Location_PDF
        {
            get { return _location_PDF; }
            set { SetValue(ref _location_PDF, value, nameof(Location_PDF)); }
        }

        private string _location_Editable;
        public string Location_Editable
        {
            get { return _location_Editable; }
            set { SetValue(ref _location_Editable, value, nameof(Location_Editable)); }
        }

        private string _isRemoved;
        public string IsRemoved
        {
            get { return _isRemoved; }
            set { SetValue(ref _isRemoved, value, nameof(IsRemoved)); }
        }

        public DocumentVersionViewModel(int documentId)
        {
            DocumentId = documentId;
            _effectiveDate = DateTime.Today;
        }
    }
}
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

        private string _versionNumber = string.Empty;
        public string VersionNumber
        {
            get { return _versionNumber; }
            set { SetValue(ref _versionNumber, value, nameof(VersionNumber)); }
        }

        private DateTime? _effectiveDate = DateTime.MinValue;
        public DateTime? EffectiveDate
        {
            get { return _effectiveDate; }
            set { SetValue(ref _effectiveDate, value, nameof(EffectiveDate)); }
        }

        private string _progress = string.Empty;
        public string Progress
        {
            get { return _progress; }
            set { SetValue(ref _progress, value, nameof(Progress)); }
        }

        private string _descriptionOfChange = string.Empty;
        public string DescriptionOfChange
        {
            get { return _descriptionOfChange; }
            set { SetValue(ref _descriptionOfChange, value, nameof(DescriptionOfChange)); }
        }

        private string _purposeOfChange = string.Empty;
        public string PurposeOfChange
        {
            get { return _purposeOfChange; }
            set { SetValue(ref _purposeOfChange, value, nameof(PurposeOfChange)); }
        }

        private string _requestor = string.Empty;
        public string Requestor
        {
            get { return _requestor; }
            set { SetValue(ref _requestor, value, nameof(Requestor)); }
        }

        private string _remarks = string.Empty;
        public string Remarks
        {
            get { return _remarks; }
            set { SetValue(ref _remarks, value, nameof(Remarks)); }
        }

        private string _location_PDF = string.Empty;
        public string Location_PDF
        {
            get { return _location_PDF; }
            set { SetValue(ref _location_PDF, value, nameof(Location_PDF)); }
        }

        private string _location_Editable = string.Empty;
        public string Location_Editable
        {
            get { return _location_Editable; }
            set { SetValue(ref _location_Editable, value, nameof(Location_Editable)); }
        }

        private string _isRemoved = string.Empty;
        public string IsRemoved
        {
            get { return _isRemoved; }
            set { SetValue(ref _isRemoved, value, nameof(IsRemoved)); }
        }

        public DocumentVersionViewModel(int documentId)
        {
            _documentId = documentId;
            _versionNumber = string.Empty;
            _effectiveDate = DateTime.Today;
            _progress = string.Empty;
            _descriptionOfChange = string.Empty;
            _purposeOfChange = string.Empty;
            _requestor = string.Empty;
            _remarks = string.Empty;
            _location_PDF = string.Empty;
            _location_Editable = string.Empty;
        }
    }
}

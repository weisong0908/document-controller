using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;

namespace DocumentController.WPF.Services
{
    public class ChangeRequestService : IChangeRequestService
    {
        private PdfDocument pdfDocument;

        public string RequestDate { get; set; }
        public string RequestorName { get; set; }
        public string RequestorDesignation { get; set; }
        public string RequestorDepartment { get; set; }
        public string CurrentDocumentTitle { get; set; }
        public string CurrentDocumentNumber { get; set; }
        public string CurrentVersionNumber { get; set; }
        public string CurrentEffectiveDate { get; set; }
        public string RevisedDocumentTitle { get; set; }
        public string RevisedDocumentNumber { get; set; }
        public string RevisedVersionNumber { get; set; }
        public string RevisedEffectiveDate { get; set; }
        public string DescriptionOfChange { get; set; }
        public string PurposeOfChange { get; set; }
        public string Remarks { get; set; }
        public string IsCompliant { get; set; }
        public string QmsReviewerName { get; set; }
        public string QmsReviewDate { get; set; }
        public string HodName { get; set; }
        public string HodApprovalDate { get; set; }
        public string HodDesignation { get; set; }

        public void ReadDcrForm(string dcrFormPath)
        {
            pdfDocument = new PdfDocument(new PdfReader(dcrFormPath));
            var dcrForm = PdfAcroForm.GetAcroForm(pdfDocument, createIfNotExist: false);
            var fields = dcrForm.GetFormFields();

            PdfFormField field;
            if (fields.TryGetValue("Request date", out field)) RequestDate = field.GetValueAsString();
            if (fields.TryGetValue("Requestor name", out field)) RequestorName = field.GetValueAsString();
            if (fields.TryGetValue("Requestor designation", out field)) RequestorDesignation = field.GetValueAsString();
            if (fields.TryGetValue("Requestor department", out field)) RequestorDepartment = field.GetValueAsString();
            if (fields.TryGetValue("Current document title", out field)) CurrentDocumentTitle = field.GetValueAsString();
            if (fields.TryGetValue("Current document number", out field)) CurrentDocumentNumber = field.GetValueAsString();
            if (fields.TryGetValue("Current version number", out field)) CurrentVersionNumber = field.GetValueAsString();
            if (fields.TryGetValue("Current effective date", out field)) CurrentEffectiveDate = field.GetValueAsString();
            if (fields.TryGetValue("Revised document title", out field)) RevisedDocumentTitle = field.GetValueAsString();
            if (fields.TryGetValue("Revised document number", out field)) RevisedDocumentNumber = field.GetValueAsString();
            if (fields.TryGetValue("Revised version number", out field)) RevisedVersionNumber = field.GetValueAsString();
            if (fields.TryGetValue("Revised effective date", out field)) RevisedEffectiveDate = field.GetValueAsString();
            if (fields.TryGetValue("Description of change", out field)) DescriptionOfChange = field.GetValueAsString();
            if (fields.TryGetValue("Purpose of change", out field)) PurposeOfChange = field.GetValueAsString();
            if (fields.TryGetValue("Remarks", out field)) Remarks = field.GetValueAsString();
            if (fields.TryGetValue("QMS compliance", out field)) IsCompliant = field.GetValueAsString();
            if (fields.TryGetValue("QMS reviewer name", out field)) QmsReviewerName = field.GetValueAsString();
            if (fields.TryGetValue("QMS review date", out field)) QmsReviewDate = field.GetValueAsString();
            if (fields.TryGetValue("HOD name", out field)) HodName = field.GetValueAsString();
            if (fields.TryGetValue("HOD approval date", out field)) HodApprovalDate = field.GetValueAsString();
            if (fields.TryGetValue("HOD designation", out field)) HodDesignation = field.GetValueAsString();
        }
    }
}

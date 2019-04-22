using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;
using DocumentController.WPF.Models;
using System.IO;
using System.Diagnostics;
using DocumentController.WPF.ViewModels;
using iText.Kernel.Font;

namespace DocumentController.WPF.Services
{
    public class DocumentChangeRequestService : IDocumentChangeRequestService
    {
        private PdfDocument pdfDocument;

        public DocumentChangeRequest ReadDcrForm(string dcrFormPath)
        {
            if (!File.Exists(dcrFormPath) || Path.GetExtension(dcrFormPath).ToLower() != ".pdf")
                return null;

            pdfDocument = new PdfDocument(new PdfReader(dcrFormPath));
            var dcrForm = PdfAcroForm.GetAcroForm(pdfDocument, createIfNotExist: false);
            var fields = dcrForm.GetFormFields();

            var documentChangeRequest = new DocumentChangeRequest();

            PdfFormField field;
            if (fields.TryGetValue("Request date", out field)) documentChangeRequest.RequestDate = field.GetValueAsString();
            if (fields.TryGetValue("Requestor name", out field)) documentChangeRequest.RequestorName = field.GetValueAsString();
            if (fields.TryGetValue("Requestor designation", out field)) documentChangeRequest.RequestorDesignation = field.GetValueAsString();
            if (fields.TryGetValue("Requestor department", out field)) documentChangeRequest.RequestorDepartment = field.GetValueAsString();
            if (fields.TryGetValue("Current document title", out field)) documentChangeRequest.CurrentDocumentTitle = field.GetValueAsString();
            if (fields.TryGetValue("Current document number", out field)) documentChangeRequest.CurrentDocumentNumber = field.GetValueAsString();
            if (fields.TryGetValue("Current version number", out field)) documentChangeRequest.CurrentVersionNumber = field.GetValueAsString();
            if (fields.TryGetValue("Current effective date", out field)) documentChangeRequest.CurrentEffectiveDate = field.GetValueAsString();
            if (fields.TryGetValue("Revised document title", out field)) documentChangeRequest.RevisedDocumentTitle = field.GetValueAsString();
            if (fields.TryGetValue("Revised document number", out field)) documentChangeRequest.RevisedDocumentNumber = field.GetValueAsString();
            if (fields.TryGetValue("Revised version number", out field)) documentChangeRequest.RevisedVersionNumber = field.GetValueAsString();
            if (fields.TryGetValue("Revised effective date", out field)) documentChangeRequest.RevisedEffectiveDate = field.GetValueAsString();
            if (fields.TryGetValue("Description of change", out field)) documentChangeRequest.DescriptionOfChange = field.GetValueAsString();
            if (fields.TryGetValue("Purpose of change", out field)) documentChangeRequest.PurposeOfChange = field.GetValueAsString();
            if (fields.TryGetValue("Remarks", out field)) documentChangeRequest.Remarks = field.GetValueAsString();
            if (fields.TryGetValue("QMS compliance", out field)) documentChangeRequest.IsCompliant = field.GetValueAsString();
            if (fields.TryGetValue("QMS reviewer name", out field)) documentChangeRequest.QmsReviewerName = field.GetValueAsString();
            if (fields.TryGetValue("QMS review date", out field)) documentChangeRequest.QmsReviewDate = field.GetValueAsString();
            if (fields.TryGetValue("HOD name", out field)) documentChangeRequest.HodName = field.GetValueAsString();
            if (fields.TryGetValue("HOD approval date", out field)) documentChangeRequest.HodApprovalDate = field.GetValueAsString();
            if (fields.TryGetValue("HOD designation", out field)) documentChangeRequest.HodDesignation = field.GetValueAsString();

            pdfDocument.Close();

            return documentChangeRequest;
        }

        public void WriteDcrForm(string dcrFormTemplatePath, string dcrFormDestinationPath, DocumentViewModel currentDocument)
        {
            pdfDocument = new PdfDocument(new PdfReader(dcrFormTemplatePath), new PdfWriter(dcrFormDestinationPath));
            var dcrForm = PdfAcroForm.GetAcroForm(pdfDocument, createIfNotExist: true);

            dcrForm.SetDefaultAppearance("true");
            dcrForm.GetField("Request date").SetValue(DateTime.Today.ToShortDateString());
            dcrForm.GetField("Requestor name").SetValue(Environment.UserName);
            dcrForm.GetField("Current document title").SetValue(currentDocument.Title);
            dcrForm.GetField("Current document number").SetValue(currentDocument.DocumentNumber);
            dcrForm.GetField("Revised document title").SetValue(currentDocument.Title);
            dcrForm.GetField("Revised document number").SetValue(currentDocument.DocumentNumber);
            dcrForm.GetField("Current version number").SetValue(currentDocument.VersionNumber);
            dcrForm.GetField("Current effective date").SetValue(currentDocument.EffectiveDate.Value.ToShortDateString());

            pdfDocument.Close();

            Process.Start(dcrFormDestinationPath);
        }
    }
}

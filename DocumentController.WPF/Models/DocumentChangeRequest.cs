using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Models
{
    public class DocumentChangeRequest
    {
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
    }
}

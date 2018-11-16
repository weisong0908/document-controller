using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Models
{
    public class Version
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string VersionNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Progress { get; set; }
        public string DescriptionOfChange { get; set; }
        public string PurposeOfChange { get; set; }
        public string Requestor { get; set; }
        public string Remarks { get; set; }
        public string Location_PDF { get; set; }
        public string Location_Editable { get; set; }
        public string IsRemoved { get; set; }
    }
}

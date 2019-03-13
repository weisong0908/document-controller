using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Models
{
    public class DocumentTitleChange
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int DocumentVersionId { get; set; }
        public string OriginalDocumentTitle { get; set; }
        public string NewDocumentTitle { get; set; }
    }
}

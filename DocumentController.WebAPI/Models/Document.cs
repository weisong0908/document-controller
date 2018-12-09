using System.Collections.Generic;

namespace DocumentController.WebAPI.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string DocumentNumber { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Function { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public IList<DocumentVersion> DocumentVersions { get; set; }

        public Document()
        {
            DocumentVersions = new List<DocumentVersion>();
        }        
    }
}
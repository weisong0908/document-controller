using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Models
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
        public string IsRemoved { get; set; }
    }
}

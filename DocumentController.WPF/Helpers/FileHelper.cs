using DocumentController.WPF.Models;
using DocumentController.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Helpers
{
    public static class FileHelper
    {
        static FileHelper()
        {

        }

        public static string GetDocumentLocation(DocumentViewModel document)
        {
            string sharedDrive = @"\\csing.navitas.local\shared\Documents\";
            string mainFolder = "";
            switch (document.Type)
            {
                case DocumentType.Policy:
                    mainFolder = @"# Curtin Singapore Corporate Policies #\" + document.Department;
                    break;
                case DocumentType.Procedure:
                    mainFolder = @"# Curtin Singapore Corporate Procedures #\" + document.Department;
                    break;
                case DocumentType.Form:
                    mainFolder = @"= Controlled Document =\Forms & Templates\" + document.Department + @"\Editable";
                    break;
                case DocumentType.WorkInstruction:
                    mainFolder = @"= Controlled Document =\Work Instructions & Guidelines\" + document.Department;
                    break;
                case DocumentType.OrganisationChart:
                    mainFolder = @"= Controlled Document =\Organisation Chart";
                    break;
            }

            string folderPath = System.IO.Path.Combine(sharedDrive, mainFolder);
            string fullPath = System.IO.Path.Combine(folderPath, $"{document.Title} - V{document.VersionNumber}");

            return fullPath;
        }
    }
}

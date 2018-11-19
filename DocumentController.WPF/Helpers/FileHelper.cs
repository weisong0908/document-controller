using DocumentController.WPF.Models;
using DocumentController.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Helpers
{
    public static class FileHelper
    {
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

            string folderPath = Path.Combine(sharedDrive, mainFolder);
            string fullPath = Path.Combine(folderPath, $"{document.Title} - V{document.VersionNumber}");

            return fullPath;
        }

        public static void GoToFile(DocumentViewModel document)
        {
            if (!File.Exists(document.Location))
            {
                WindowHelper.ShowMessageBox("The path to the file is either invalid or the file has been removed. Please navigate to the file manually.", "File not found");
                return;
            }

            Process.Start(document.Location);
        }
    }
}

using DocumentController.WPF.Models;
using DocumentController.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentController.WPF.Helpers
{
    public class FileHelper: IFileHelper
    {
        public string GetDocumentLocation(DocumentViewModel document)
        {
            string sharedDrive = (System.Windows.Application.Current as App).SharedDrive;
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
            string fullPath = Path.Combine(folderPath, $"{document.Title} - V{document.VersionNumber}.pdf");

            return fullPath;
        }

        public void GoToFile(DocumentViewModel document)
        {
            if (!File.Exists(document.Location))
            {
                var windowHelper = (System.Windows.Application.Current as App).WindowHelper;
                windowHelper.Alert("The path to the file is either invalid or the file has been removed. Please navigate to the file manually.", "File not found");
                return;
            }

            Process.Start(Path.GetDirectoryName(document.Location));
        }

        private (string publicPDF, string publicEditable, string privateCurrentPDF, string privateCurrentEditable, string privateObselete) GetUploadPaths(DocumentViewModel document)
        {
            string sharedDrive = (System.Windows.Application.Current as App).SharedDrive;
            string archiveFolder = (System.Windows.Application.Current as App).ArchivedFolder;
            string publicPDF = sharedDrive;
            string publicEditable = sharedDrive;
            string privateCurrentPDF = archiveFolder;
            string privateCurrentEditable = archiveFolder;
            string privateObselete = archiveFolder;

            switch (document.Type)
            {
                case DocumentType.Policy:
                    publicPDF += Path.Combine(@"# Curtin Singapore Corporate Policies #", document.Department);
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\Pol\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\Pol\Editable");
                    privateObselete += Path.Combine(document.Department, @"Obselete\Pol");
                    break;
                case DocumentType.Procedure:
                    publicPDF += Path.Combine(@"# Curtin Singapore Corporate Procedures #", document.Department);
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\Pro\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\Pro\Editable");
                    privateObselete += Path.Combine(document.Department, @"Obselete\Pro");
                    break;
                case DocumentType.Form:
                    publicPDF += Path.Combine(@"= Controlled Document =\Forms & Templates", document.Department + @"\PDF");
                    publicEditable += Path.Combine(@"= Controlled Document =\Forms & Templates", document.Department + @"\Editable");
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\F & T\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\F & T\Editable");
                    privateObselete += Path.Combine(document.Department, @"Obselete\F & T");
                    break;
                case DocumentType.WorkInstruction:
                    publicPDF += Path.Combine(@"= Controlled Document =\Work Instructions & Guidelines", document.Department);
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\WI & GL\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\WI & GL\Editable");
                    privateObselete += Path.Combine(document.Department, @"Obselete\WI & GL");
                    break;
                case DocumentType.OrganisationChart:
                    publicPDF += Path.Combine(@"= Controlled Document =", "Organisation Chart");
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\Organisation Chart\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\Organisation Chart\Editable");
                    privateObselete += Path.Combine(document.Department, @"Obselete\Organisation Chart");
                    break;
            }

            return (publicPDF, publicEditable, privateCurrentPDF, privateCurrentEditable, privateObselete);
        }

        private void DeleteFile(DocumentViewModel document, string folder)
        {
            var filesToDelete = Directory.GetFiles(folder, document.Title + "*", SearchOption.TopDirectoryOnly);

            if (filesToDelete.Length == 0)
                return;

            File.Delete(filesToDelete[0]);
        }

        private void MoveFile(DocumentViewModel document, string sourceFolder, string destinationFolder)
        {
            var searchResults = Directory.GetFiles(sourceFolder, document.Title + "*", SearchOption.TopDirectoryOnly);

            if (searchResults.Length == 0)
                return;

            var fileToMove = Path.GetFileName(searchResults[0]);

            if (File.Exists(Path.Combine(destinationFolder, fileToMove)))
                File.Delete(Path.Combine(destinationFolder, fileToMove));

            File.Move(Path.Combine(sourceFolder, fileToMove), Path.Combine(destinationFolder, fileToMove));

            Process.Start(sourceFolder);
            Process.Start(destinationFolder);
        }

        private void CopyFile(DocumentViewModel document, DocumentVersionViewModel documentVersion, string sourceFileName, string destinationFolder)
        {
            var fileToCopy = document.Title + " - V" + documentVersion.VersionNumber + Path.GetExtension(sourceFileName);
            var destinationFileName = Path.Combine(destinationFolder, fileToCopy);

            if (File.Exists(sourceFileName))
                File.Copy(sourceFileName, destinationFileName, overwrite: true);

            Process.Start(destinationFolder);
        }

        public void UpdateFiles(DocumentViewModel document, DocumentVersionViewModel documentVersion)
        {
            var (publicPDF, publicEditable, privateCurrentPDF, privateCurrentEditable, privateObselete) = GetUploadPaths(document);

            DeleteFile(document, publicPDF);
            CopyFile(document, documentVersion, documentVersion.Location_PDF, publicPDF);
            MoveFile(document, privateCurrentPDF, privateObselete);
            CopyFile(document, documentVersion, documentVersion.Location_PDF, privateCurrentPDF);
            MoveFile(document, privateCurrentEditable, privateObselete);
            CopyFile(document, documentVersion, documentVersion.Location_Editable, privateCurrentEditable);

            if (document.Type == DocumentType.Form)
            {
                DeleteFile(document, publicEditable);
                CopyFile(document, documentVersion, documentVersion.Location_Editable, publicEditable);
            }
        }

        public string GetFilePath(FileType fileType)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = $"Get File Path";
                if (fileType == FileType.PDF)
                    fileDialog.Filter = "PDF files (*.PDF)|*.PDF";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    return fileDialog.FileName;
                }

                return string.Empty;
            }
        }
    }
}

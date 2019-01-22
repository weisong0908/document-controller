﻿using DocumentController.WPF.Models;
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
    public partial class FileHelper: IFileHelper
    {
        readonly string constitutionsPublicPath = @"# Constitution and Terms of Reference #\";
        readonly string policiesPublicPath = @"# Curtin Singapore Corporate Policies #\";
        readonly string proceduresPublicPath = @"# Curtin Singapore Corporate Procedures #\";
        readonly string formsPublicPath = @"= Controlled Document =\Forms & Templates\";
        readonly string workInstructionsPublicPath = @"= Controlled Document =\Work Instructions & Guidelines\";
        readonly string organisationChartPublicPath = @"= Controlled Document =\Organisation Chart";

        public string GetDocumentLocation(DocumentViewModel document)
        {
            string sharedDrive = (System.Windows.Application.Current as App).SharedDrive;
            string mainFolder = "";
            switch (document.Type)
            {
                case DocumentType.Constitution:
                    mainFolder = constitutionsPublicPath;
                    break;
                case DocumentType.Policy:
                    mainFolder = policiesPublicPath + document.Department;
                    break;
                case DocumentType.Procedure:
                    mainFolder = proceduresPublicPath + document.Department;
                    break;
                case DocumentType.Form:
                    mainFolder = formsPublicPath + document.Department + @"\Editable";
                    break;
                case DocumentType.WorkInstruction:
                    mainFolder = workInstructionsPublicPath + document.Department;
                    break;
                case DocumentType.OrganisationChart:
                    mainFolder = organisationChartPublicPath;
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

        private (string publicPDF, string publicEditable, string privateCurrentPDF, string privateCurrentEditable, string privateObsolete) GetUploadPaths(DocumentViewModel document)
        {
            string publicDrive = (System.Windows.Application.Current as App).SharedDrive;
            string archiveFolder = (System.Windows.Application.Current as App).ArchivedFolder;
            string publicPDF = publicDrive;
            string publicEditable = publicDrive;
            string privateCurrentPDF = archiveFolder;
            string privateCurrentEditable = archiveFolder;
            string privateObsolete = archiveFolder;

            switch (document.Type)
            {
                case DocumentType.Constitution:
                    publicPDF += Path.Combine(constitutionsPublicPath);
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\Cons\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\Cons\Editable");
                    privateObsolete += Path.Combine(document.Department, @"Obsolete\Cons");
                    break;
                case DocumentType.Policy:
                    publicPDF += Path.Combine(policiesPublicPath, document.Department);
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\Pol\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\Pol\Editable");
                    privateObsolete += Path.Combine(document.Department, @"Obsolete\Pol");
                    break;
                case DocumentType.Procedure:
                    publicPDF += Path.Combine(proceduresPublicPath, document.Department);
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\Pro\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\Pro\Editable");
                    privateObsolete += Path.Combine(document.Department, @"Obsolete\Pro");
                    break;
                case DocumentType.Form:
                    publicPDF += Path.Combine(formsPublicPath, document.Department + @"\PDF");
                    publicEditable += Path.Combine(formsPublicPath, document.Department + @"\Editable");
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\F & T\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\F & T\Editable");
                    privateObsolete += Path.Combine(document.Department, @"Obsolete\F & T");
                    break;
                case DocumentType.WorkInstruction:
                    publicPDF += Path.Combine(workInstructionsPublicPath, document.Department);
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\WI & GL\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\WI & GL\Editable");
                    privateObsolete += Path.Combine(document.Department, @"Obsolete\WI & GL");
                    break;
                case DocumentType.OrganisationChart:
                    publicPDF += organisationChartPublicPath;
                    privateCurrentPDF += Path.Combine(document.Department, @"Current\Organisation Chart\PDF");
                    privateCurrentEditable += Path.Combine(document.Department, @"Current\Organisation Chart\Editable");
                    privateObsolete += Path.Combine(document.Department, @"Obsolete\Organisation Chart");
                    break;
            }

            return (publicPDF, publicEditable, privateCurrentPDF, privateCurrentEditable, privateObsolete);
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

        public void UpdateFiles(DocumentViewModel document, DocumentVersionViewModel documentVersion = null, UpdateFilesMethod updateFilesMethod = UpdateFilesMethod.UpdateVersion)
        {
            var (publicPDF, publicEditable, privateCurrentPDF, privateCurrentEditable, privateObselete) = GetUploadPaths(document);

            DeleteFile(document, publicPDF);
            if(updateFilesMethod == UpdateFilesMethod.UpdateVersion)
                CopyFile(document, documentVersion, documentVersion.Location_PDF, publicPDF);
            MoveFile(document, privateCurrentPDF, privateObselete);
            if (updateFilesMethod == UpdateFilesMethod.UpdateVersion)
                CopyFile(document, documentVersion, documentVersion.Location_PDF, privateCurrentPDF);
            MoveFile(document, privateCurrentEditable, privateObselete);
            if (updateFilesMethod == UpdateFilesMethod.UpdateVersion)
                CopyFile(document, documentVersion, documentVersion.Location_Editable, privateCurrentEditable);

            if (document.Type == DocumentType.Form)
            {
                DeleteFile(document, publicEditable);
                if (updateFilesMethod == UpdateFilesMethod.UpdateVersion)
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

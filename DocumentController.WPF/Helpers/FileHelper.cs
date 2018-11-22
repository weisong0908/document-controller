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

        private static (string publicPDF, string publicEditable, string privateCurrentPDF, string privateCurrentEditable, string privateObselete) GetUploadPaths(DocumentViewModel document)
        {
            //string sharedDrive = @"\\csing.navitas.local\shared\Documents\";
            //string archivePath = @"\\csing.navitas.local\shared\Documents\Quality Assurance\#QA & COMPLIANCE Dept Functions#\Controlled Document";
            string sharedDrive = @"C:\Users\weisong.teng\Desktop\S\";
            string archivePath = @"C:\Users\weisong.teng\Desktop\S\Quality Assurance\#QA & COMPLIANCE Dept Functions#\Controlled Document";
            string publicPDF = sharedDrive + @"\";
            string publicEditable = sharedDrive + @"\";
            string privateCurrentPDF = archivePath + @"\";
            string privateCurrentEditable = archivePath + @"\";
            string privateObselete = archivePath + @"\";

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

        private static void DeleteFile(DocumentViewModel document, string folder)
        {
            var filesToDelete = Directory.GetFiles(folder, document.Title + "*", SearchOption.TopDirectoryOnly);

            if (filesToDelete.Length == 0)
                return;

            File.Delete(filesToDelete[0]);
        }

        private static void MoveFile(DocumentViewModel document, string sourceFolder, string destinationFolder)
        {
            var searchResults = Directory.GetFiles(sourceFolder, document.Title + "*", SearchOption.TopDirectoryOnly);

            if (searchResults.Length == 0)
                return;

            var fileToMove = Path.GetFileName(searchResults[0]);

            File.Move(Path.Combine(sourceFolder, fileToMove), Path.Combine(destinationFolder, fileToMove));

            Process.Start(sourceFolder);
            Process.Start(destinationFolder);
        }

        private static void CopyFile(DocumentViewModel document, DocumentVersionViewModel documentVersion, string sourceFileName, string destinationFolder)
        {
            var fileToCopy = document.Title + " - V" + documentVersion.VersionNumber + Path.GetExtension(sourceFileName);
            var destinationFileName = Path.Combine(destinationFolder, fileToCopy);

            if (File.Exists(sourceFileName))
                File.Copy(sourceFileName, destinationFileName);

            Process.Start(destinationFolder);
        }

        public static void UpdateFiles(DocumentViewModel document, DocumentVersionViewModel documentVersion)
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

        public static string GetFilePath(FileType fileType)
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

        public enum FileType
        {
            PDF, Editable
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DocumentController.WPF.Models;

namespace DocumentController.WPF.Services
{
    public class LocalDocumentVersionService : IDocumentVersionService
    {
        private OleDbConnection dbConnection;
        private OleDbDataReader dataReader;
        private OleDbCommand command;

        public LocalDocumentVersionService()
        {
            dbConnection = new OleDbConnection((Application.Current as App).ConnectionString);
        }

        public async Task<IEnumerable<DocumentVersion>> GetAllVersionsByDocumentId(int documentId)
        {
            dbConnection.Open();
            command = new OleDbCommand("SELECT * FROM Versions WHERE Document_ID =" + documentId, dbConnection);
            dataReader = command.ExecuteReader();

            var documentVersions = new List<DocumentVersion>();

            while (await dataReader.ReadAsync())
            {
                documentVersions.Add(new DocumentVersion()
                {
                    Id = int.Parse(dataReader["ID"].ToString()),
                    DocumentId = int.Parse(dataReader["Document_ID"].ToString()),
                    VersionNumber = dataReader["Version"].ToString(),
                    EffectiveDate = (dataReader["Effective_Date"] == DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dataReader["Effective_Date"].ToString()),
                    Progress = dataReader["Update_Status"].ToString(),
                    DescriptionOfChange = dataReader["Description_of_Change"].ToString(),
                    PurposeOfChange = dataReader["Purpose_of_Change"].ToString(),
                    Requestor = dataReader["Requestor"].ToString(),
                    Remarks = dataReader["Remarks"].ToString(),
                    Location_PDF = dataReader["Location_PDF"].ToString(),
                    Location_Editable = dataReader["Location_Editable"].ToString(),
                    IsRemoved = dataReader["Is_Removed"].ToString().ToLower()
                });
            }

            dataReader.Close();
            dbConnection.Close();

            return documentVersions.Where(dv => dv.IsRemoved != "true");
        }

        public async Task<DocumentVersion> AddNewDocumentVersion(DocumentVersion documentVersion)
        {
            string sql = "INSERT INTO Versions " +
                "(Document_ID, Version, Effective_Date, Update_Status, Description_of_Change, Purpose_of_Change, " +
                "Requestor, Remarks, Location_PDF, Location_Editable) VALUES " +
                "(@DocumentId, @VersionNumber, @EffectiveDate, @Progress, @DescriptionOfChange, @PurposeOfChange, " +
                "@Requestor, @Remarks, @Location_PDF, @Location_Editable)";

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            OleDbParameter documentId = new OleDbParameter("@DocumentId", documentVersion.DocumentId);
            OleDbParameter versionNumber = new OleDbParameter("@VersionNumber", documentVersion.VersionNumber);
            OleDbParameter effectiveDate = new OleDbParameter("@EffectiveDate", documentVersion.EffectiveDate);
            OleDbParameter progress = new OleDbParameter("@Progress", documentVersion.Progress);
            OleDbParameter descriptionOfChange = new OleDbParameter("@DescriptionOfChange", documentVersion.DescriptionOfChange ?? string.Empty);
            OleDbParameter purposeOfChange = new OleDbParameter("@PurposeOfChange", documentVersion.PurposeOfChange ?? string.Empty);
            OleDbParameter requestor = new OleDbParameter("@Requestor", documentVersion.Requestor ?? string.Empty);
            OleDbParameter remarks = new OleDbParameter("@Remarks", documentVersion.Remarks ?? string.Empty);
            OleDbParameter location_PDF = new OleDbParameter("@Location_PDF", documentVersion.Location_PDF ?? string.Empty);
            OleDbParameter location_Editable = new OleDbParameter("@Location_Editable", documentVersion.Location_Editable ?? string.Empty);

            command.Parameters.Add(documentId);
            command.Parameters.Add(versionNumber);
            command.Parameters.Add(effectiveDate);
            command.Parameters.Add(progress);
            command.Parameters.Add(descriptionOfChange);
            command.Parameters.Add(purposeOfChange);
            command.Parameters.Add(requestor);
            command.Parameters.Add(remarks);
            command.Parameters.Add(location_PDF);
            command.Parameters.Add(location_Editable);

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();

            return documentVersion;
        }

        public async Task<DocumentVersion> UpdateDocumentVersion(DocumentVersion documentVersion)
        {
            string sql = "UPDATE Versions SET " +
                "Document_ID = @DocumentId, " +
                "Version = @VersionNumber, " +
                "Effective_Date = @EffectiveDate, " +
                "Update_Status = @Progress, " +
                "Description_of_Change = @DescriptionOfChange, " +
                "Purpose_of_Change = @PurposeOfChange, " +
                "Requestor = @Requestor, " +
                "Remarks = @Remarks, " +
                "Location_PDF = @Location_PDF, " +
                "Location_Editable = @Location_Editable " +
                "WHERE ID = " + documentVersion.Id;

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            OleDbParameter documentId = new OleDbParameter("@DocumentId", documentVersion.DocumentId);
            OleDbParameter versionNumber = new OleDbParameter("@VersionNumber", documentVersion.VersionNumber);
            OleDbParameter effectiveDate = new OleDbParameter("@EffectiveDate", documentVersion.EffectiveDate);
            OleDbParameter progress = new OleDbParameter("@Progress", documentVersion.Progress);
            OleDbParameter descriptionOfChange = new OleDbParameter("@DescriptionOfChange", documentVersion.DescriptionOfChange ?? string.Empty);
            OleDbParameter purposeOfChange = new OleDbParameter("@PurposeOfChange", documentVersion.PurposeOfChange ?? string.Empty);
            OleDbParameter requestor = new OleDbParameter("@Requestor", documentVersion.Requestor ?? string.Empty);
            OleDbParameter remarks = new OleDbParameter("@Remarks", documentVersion.Remarks ?? string.Empty);
            OleDbParameter locationPDF = new OleDbParameter("@LocationPDF", documentVersion.Location_PDF ?? string.Empty);
            OleDbParameter locationEditable = new OleDbParameter("@LocationEditable", documentVersion.Location_Editable ?? string.Empty);

            command.Parameters.Add(documentId);
            command.Parameters.Add(versionNumber);
            command.Parameters.Add(effectiveDate);
            command.Parameters.Add(progress);
            command.Parameters.Add(descriptionOfChange);
            command.Parameters.Add(purposeOfChange);
            command.Parameters.Add(requestor);
            command.Parameters.Add(remarks);
            command.Parameters.Add(locationPDF);
            command.Parameters.Add(locationEditable);

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();

            return documentVersion;
        }

        public async void RemoveDocumentVersion(DocumentVersion documentVersion)
        {
            string sql = "UPDATE Versions SET Is_Removed = @IsRemoved WHERE ID = " + documentVersion.Id;

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            OleDbParameter isRemoved = new OleDbParameter("@IsRemoved", "true");
            command.Parameters.Add(isRemoved);

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();
        }
    }
}

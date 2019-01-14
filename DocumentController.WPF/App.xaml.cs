using AutoMapper;
using DocumentController.WPF.Mapping;
using DocumentController.WPF.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentController.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public string ConnectionString { get; } = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\csing.navitas.local\shared\Documents\z-Public\QA and Compliance\Controlled Document\Controlled Document Master List.mdb;Persist Security Info=False;Jet OLEDB:Database Password=1234;";
        public string ConnectionString { get; } = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\weisong.teng\Desktop\Controlled Document Master List.mdb;Persist Security Info=False;Jet OLEDB:Database Password=1234;";
        public string SharedDrive { get; set; } = @"\\csing.navitas.local\shared\Documents\";
        //public string SharedDrive { get;} = @"C:\Users\weisong.teng\Desktop\S\";
        public string ArchivedFolder { get; set; } = @"\\csing.navitas.local\shared\Documents\Quality Assurance\#QA & COMPLIANCE Dept Functions#\Controlled Document\";
        //public string ArchivedFolder { get; } = @"C:\Users\weisong.teng\Desktop\S\Quality Assurance\#QA & COMPLIANCE Dept Functions#\Controlled Document\";

        public IDocumentService DocumentService;
        public IDocumentVersionService DocumentVersionService;

        protected override void OnStartup(StartupEventArgs e)
        {
            DocumentService = new LocalDocumentService();
            DocumentVersionService = new LocalDocumentVersionService();
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());

            var startupWindow = new Views.DocumentsWindow();
            startupWindow.Show();
        }
    }
}

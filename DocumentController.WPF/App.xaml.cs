using AutoMapper;
using DocumentController.WPF.Helpers;
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
        public string ApiEndPointBaseAddress { get; } = @"http://documentcontroller.weisong.me/api/";
        private string _databaseLocation = @"\\csing.navitas.local\shared\Documents\Quality Assurance - Shared\Document Control\Controlled Document Master List - Copy.mdb";
        public string ConnectionString { get { return $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={_databaseLocation};Persist Security Info=False;Jet OLEDB:Database Password=1234;"; } }
        public string SharedDrive { get; set; } = @"\\csing.navitas.local\shared\Documents\";
        public string ArchivedFolder { get; set; } = @"\\csing.navitas.local\shared\Documents\Quality Assurance\#QA & COMPLIANCE Dept Functions#\Controlled Document\";

        public IDocumentService DocumentService;
        public IDocumentVersionService DocumentVersionService;
        public IAdminUserService AdminUserService;
        public IDocumentChangeRequestService ChangeRequestService;
        public IFileHelper FileHelper;
        public IWindowHelper WindowHelper;
        public IMapper Mapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            DocumentService = new LocalDocumentService();
            DocumentVersionService = new LocalDocumentVersionService();
            AdminUserService = new LocalAdminUserService();
            ChangeRequestService = new DocumentChangeRequestService();
            FileHelper = new FileHelper(_databaseLocation);
            WindowHelper = new WindowHelper();
            Mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();

            var startupWindow = new Views.DocumentsWindow();
            startupWindow.Show();
        }
    }
}

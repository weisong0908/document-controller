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
        public IDocumentService DocumentService;
        public IDocumentVersionService DocumentVersionService;

        protected override void OnStartup(StartupEventArgs e)
        {
            DocumentService = new FakeDocumentService();
            DocumentVersionService = new FakeDocumentVersionService();
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());

            var startupWindow = new Views.DocumentsWindow();
            startupWindow.Show();
        }
    }
}

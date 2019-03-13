using DocumentController.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DocumentController.WPF.Views
{
    /// <summary>
    /// Interaction logic for DocumentVersionWindow.xaml
    /// </summary>
    public partial class DocumentVersionsWindow : Window
    {
        public DocumentVersionsWindowViewModel ViewModel { get { return DataContext as DocumentVersionsWindowViewModel; } set { DataContext = value; } }

        public DocumentVersionsWindow(object selectedDocument)
        {
            InitializeComponent();

            var documentVersionService = (Application.Current as App).DocumentVersionService;
            var documentTitleChangeService = (Application.Current as App).DocumentTitleChangeService;
            var fileHelper = (Application.Current as App).FileHelper;
            var windowHelper = (Application.Current as App).WindowHelper;
            var mapper = (Application.Current as App).Mapper;
            ViewModel = new DocumentVersionsWindowViewModel(documentVersionService, documentTitleChangeService, fileHelper, windowHelper, mapper);
            ViewModel.OnStartUp(selectedDocument as DocumentViewModel);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveDocumentVersion();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateNewDocumentVersion();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveDocumentVersion();
        }

        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UploadDocument();
        }

        private void FindPdf_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.BrowsePDFFile();
        }

        private void FindEditable_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.BrowseEditableFile();
        }
    }
}

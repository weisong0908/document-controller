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

            ViewModel = new DocumentVersionsWindowViewModel(documentVersionService);
            ViewModel.OnStartUp(selectedDocument as DocumentViewModel);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnSaveVersion();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnNewVersion();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnRemoveVersion();
        }

        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UploadDocument();
        }

        private void FindPdf_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLocatePdf();
        }

        private void FindEditable_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLocateEditable();
        }
    }
}

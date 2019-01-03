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
    /// Interaction logic for DocumentsWindow.xaml
    /// </summary>
    public partial class DocumentsWindow : Window
    {
        public DocumentsWindowViewModel ViewModel { get { return DataContext as DocumentsWindowViewModel; } set { DataContext = value; } }

        public DocumentsWindow()
        {
            InitializeComponent();

            var documentService = (Application.Current as App).DocumentService;
            var documentVersionService = (Application.Current as App).DocumentVersionService;
            var fileHelper = (Application.Current as App).FileHelper;
            var windowHelper = (Application.Current as App).WindowHelper;
            var mapper = (Application.Current as App).Mapper;
            ViewModel = new DocumentsWindowViewModel(documentService, documentVersionService, fileHelper, windowHelper, mapper);
        }

        protected override void OnActivated(EventArgs e)
        {
            ViewModel.OnActivated();
            base.OnActivated(e);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.FilterDocuments((e.Source as TextBox).Text.ToString());
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var selectedDocument = e.AddedItems[0] as DocumentViewModel;

            ViewModel.SelectDocument(selectedDocument);
        }

        private void EditVersion_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToVersionsWindow();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnNewDocument();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ViewModel.OnActivated();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnNavigateToDocumentLocation();
        }

        private void DatabaseBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnBackUpDatabase();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.GoToVersionsWindow();
        }
    }
}

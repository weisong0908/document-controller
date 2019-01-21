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
    /// Interaction logic for NewDocumentWindow.xaml
    /// </summary>
    public partial class NewDocumentWindow : Window
    {
        public NewDocumentWindowViewModel ViewModel { get { return DataContext as NewDocumentWindowViewModel; } set { DataContext = value; } }

        public NewDocumentWindow()
        {
            InitializeComponent();

            var documentService = (Application.Current as App).DocumentService;
            var windowHelper = (Application.Current as App).WindowHelper;
            var mapper = (Application.Current as App).Mapper;
            ViewModel = new NewDocumentWindowViewModel(documentService, windowHelper, mapper);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateNewDocument();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelNewDocument();
        }

        private void Departments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.OnDepartmentChanged();
        }
    }
}

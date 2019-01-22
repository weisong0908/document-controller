using DocumentController.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentController.WPF.Helpers
{
    public class WindowHelper: IWindowHelper
    {
        public void Alert(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }

        public bool Confirmation(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
                return true;

            return false;
        }

        public void ShowWindow(WindowType windowType, object parameter = null)
        {
            Window window = null;

            switch (windowType)
            {
                case WindowType.DocumentVersionWindow:
                    window = Application.Current.Windows.OfType<DocumentVersionsWindow>().SingleOrDefault();

                    if (window == null)
                    {
                        window = new DocumentVersionsWindow(parameter);
                        window.ShowDialog();
                    }
                    break;

                case WindowType.NewDocumentWindow:
                    window = Application.Current.Windows.OfType<NewDocumentWindow>().SingleOrDefault();

                    if(window == null)
                    {
                        window = new NewDocumentWindow();
                        window.ShowDialog();
                    }
                    break;
            }
        }

        public void CloseWindow()
        {
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive == true);
            window.Close();
        }
    }
}

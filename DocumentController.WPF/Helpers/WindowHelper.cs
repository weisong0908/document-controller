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
            }
        }
    }
}

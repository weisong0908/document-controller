using DocumentController.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentController.WPF.Helpers
{
    public static class WindowHelper
    {
        public static void ShowWindow(WindowType windowType, object parameter = null)
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

        public enum WindowType
        {
            DocumentVersionWindow
        }
    }
}

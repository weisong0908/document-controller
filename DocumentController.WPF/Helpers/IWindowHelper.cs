using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Helpers
{
    public interface IWindowHelper
    {
        void Alert(string message, string caption);

        void ShowWindow(WindowType windowType, object parameter = null);

        void CloseWindow();
    }
}

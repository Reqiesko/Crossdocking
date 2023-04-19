using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossdocking.Services
{
    public class ChangeWindowSizeService
    {
        private MainWindow _mainWindow;
        public ChangeWindowSizeService(MainWindow mw)
        {
            _mainWindow = mw;
        }
        public void ChangeSize(double height, double width)
        {
            _mainWindow.Width = width;
            _mainWindow.Height = height;
        }
    }
}

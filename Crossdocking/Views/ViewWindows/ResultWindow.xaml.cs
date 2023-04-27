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

namespace Crossdocking.Views.ViewWindows
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public string Result { get; set; }
        public int LoaderCount { get; set; }
        public int BeltCount { get; set; }
        public int GatesCount { get; set; }
        public ResultWindow(string resultImagePaths, int loaders, int belts, int gatesCount)
        {
            InitializeComponent();
            Result = resultImagePaths;
            LoaderCount = loaders;
            BeltCount = belts;
            DataContext = this;
            GatesCount = gatesCount;
        }
    }
}

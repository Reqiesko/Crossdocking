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
using DAL;

namespace Crossdocking.Views.ViewWindows
{
    /// <summary>
    /// Логика взаимодействия для TerminalWindow.xaml
    /// </summary>
    public partial class TerminalWindow : Window
    {
        public Terminal Terminal { get; private set; }
        public TerminalWindow(Terminal terminal)
        {
            InitializeComponent();
            Terminal = terminal;
            DataContext = Terminal;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

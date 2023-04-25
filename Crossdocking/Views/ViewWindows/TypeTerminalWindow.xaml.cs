using DAL;
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
    /// Логика взаимодействия для TypeTerminalWindow.xaml
    /// </summary>
    public partial class TypeTerminalWindow : Window
    {
        public TypeTerminal TypeTerminal { get; private set; }
        public TypeTerminalWindow(TypeTerminal terminal)
        {
            InitializeComponent();
            TypeTerminal = terminal;
            DataContext = TypeTerminal;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

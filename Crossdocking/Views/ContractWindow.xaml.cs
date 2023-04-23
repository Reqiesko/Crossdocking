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

namespace Crossdocking.Views
{
    /// <summary>
    /// Логика взаимодействия для ContractWindow.xaml
    /// </summary>
    public partial class ContractWindow : Window
    {
        public Contract Contract { get; private set; }
        public ContractWindow(Contract contract)
        {
            InitializeComponent();
            Contract = contract;
            DataContext = Contract;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

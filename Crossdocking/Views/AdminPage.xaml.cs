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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crossdocking.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : UserControl
    {
        private enum Tables
        {
            ContractsTable,
            TerminalsTable,
            TypeTerminalsTable,
            UsersTable,
        }

        private Tables CurrentTable { get; set; }

        private DataGrid CurrentGrid { get; set; }

        public AdminPage()
        {
            InitializeComponent();
            CurrentTable = new Tables();
            CurrentTable = Tables.ContractsTable;
            CurrentGrid = ContractsGrid;
        }

        public void ShowContractsTableCommand(object sender, RoutedEventArgs routedEventArgs)
        {
            if (CurrentTable == Tables.ContractsTable)
            {
                return;
            }
            else
            {
                CurrentGrid.Visibility = Visibility.Hidden;
                CurrentTable = Tables.ContractsTable;
                CurrentGrid = ContractsGrid;
                CurrentGrid.Visibility = Visibility.Visible;
            }
        }

        private void ShowTerminalsTableCommand(object sender, RoutedEventArgs e)
        {
            if (CurrentTable == Tables.TerminalsTable)
            {
                return;
            }
            else
            {
                CurrentGrid.Visibility = Visibility.Hidden;
                CurrentTable = Tables.TerminalsTable;
                CurrentGrid = TerminalsGrid;
                CurrentGrid.Visibility = Visibility.Visible;
            }
        }

        private void ShowTypeTerminalsTableCommand(object sender, RoutedEventArgs e)
        {
            if (CurrentTable == Tables.TypeTerminalsTable)
            {
                return;
            }
            else
            {
                CurrentGrid.Visibility = Visibility.Hidden;
                CurrentTable = Tables.TypeTerminalsTable;
                CurrentGrid = TypeTerminalsGrid;
                CurrentGrid.Visibility = Visibility.Visible;
            }
        }

        private void ShowUsersTableCommand(object sender, RoutedEventArgs e)
        {
            if (CurrentTable == Tables.UsersTable)
            {
                return;
            }
            else
            {
                CurrentGrid.Visibility = Visibility.Hidden;
                CurrentTable = Tables.UsersTable;
                CurrentGrid = UsersGrid;
                CurrentGrid.Visibility = Visibility.Visible;
            }
        }
    }
}

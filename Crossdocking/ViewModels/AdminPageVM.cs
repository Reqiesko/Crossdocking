using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Crossdocking.Services;
using Crossdocking.Views.ViewWindows;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace Crossdocking.ViewModels
{
    public class AdminPageVM : BaseVM.ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        public ObservableCollection<Contract> Contracts { get; set; }
        public ObservableCollection<Terminal> Terminals { get; set; }
        public ObservableCollection<TypeTerminal> TypeTerminals { get; set; }
        public ObservableCollection<User> Users { get; set; }

        private readonly NavigationService _navigationService;

        public object AddCommand { get; set; }
        public string AddNewNoteText { get; set; }

        private RelayCommand _showInformationCommand;
        private RelayCommand _backToLoginPageCommand;

        private RelayCommand _addContractCommand;
        private RelayCommand _deleteContractCommand;
        private RelayCommand _editContractCommand;

        private RelayCommand _addTerminalCommand;
        private RelayCommand _deleteTerminalCommand;
        private RelayCommand _editTerminalCommand;

        private RelayCommand _addTypeTerminalCommand;
        private RelayCommand _deleteTypeTerminalCommand;
        private RelayCommand _editTypeTerminalCommand;

        private RelayCommand _addUserCommand;
        private RelayCommand _editUserCommand;
        private RelayCommand _deleteUserCommand;

        private RelayCommand _contractChangeAddCommand;
        private RelayCommand _terminalChangeAddCommand;
        private RelayCommand _typeTerminalChangeAddCommand;
        private RelayCommand _userChangeAddCommand;

        public AdminPageVM(NavigationService navigationService)
        {
            _dbContext = new AppDbContext();
            _navigationService = navigationService;
            try
            {
                _dbContext.Contracts.Load();
                _dbContext.Terminals.Load();
                _dbContext.TypeTerminals.Load();
                _dbContext.Users.Load();

                Contracts = _dbContext.Contracts.Local.ToObservableCollection();
                Terminals = _dbContext.Terminals.Local.ToObservableCollection();
                TypeTerminals = _dbContext.TypeTerminals.Local.ToObservableCollection();
                Users = _dbContext.Users.Local.ToObservableCollection();
                AddNewNoteText = "Контракт";
                AddCommand = AddContractCommand;
            }
            catch { }
        }

        public RelayCommand BackToLoginPageCommand
        {
            get
            {
                return _backToLoginPageCommand ??= new RelayCommand(o =>
                {
                    _navigationService.CurrentViewModel = new LoginPageVM(_navigationService);
                });
            }
        }

        public RelayCommand ShowInformationCommand
        {
            get
            {
                return _showInformationCommand ??= new RelayCommand(o =>
                {

                });
            }
        }

        #region ChangeAddCommands
        public RelayCommand ContractChangeAddCommand
        {
            get
            {
                return _contractChangeAddCommand ??= new RelayCommand((o) =>
                {
                    AddCommand = AddContractCommand;
                    AddNewNoteText = "Контракт";
                });
            }
        }
        public RelayCommand TerminalChangeAddCommand
        {
            get
            {
                return _terminalChangeAddCommand ??= new RelayCommand((o) =>
                {
                    AddCommand = AddTerminalCommand;
                    AddNewNoteText = "Терминал";
                });
            }
        }
        public RelayCommand TypeTerminalChangeAddCommand
        {
            get
            {
                return _typeTerminalChangeAddCommand ??= new RelayCommand((o) =>
                {
                    AddCommand = AddTypeTerminalCommand;
                    AddNewNoteText = "Тип терминала";
                });
            }
        }
        public RelayCommand UserChangeAddCommand
        {
            get
            {
                return _userChangeAddCommand ??= new RelayCommand((o) =>
                {
                    AddCommand = AddUserCommand;
                    AddNewNoteText = "Пользователя";
                });
            }
        }
        #endregion

        #region Contract RelayCommands
        public RelayCommand AddContractCommand
        {
            get
            {
                return _addContractCommand ??= new RelayCommand((o) =>
                {
                    ContractWindow contractWindow = new ContractWindow(new Contract());
                    if (contractWindow.ShowDialog() == true)
                    {
                        Contract contract = contractWindow.Contract;
                        _dbContext.Contracts.Add(contract);
                        try
                        {
                            _dbContext.SaveChanges();
                        }
                        catch
                        {
                            _dbContext.Contracts.Remove(contract);
                            MessageBox.Show("Заполните все поля!",
                                "Ошибка!",
                                MessageBoxButton.OK);
                        }
                    }
                });
            }
        }
        public RelayCommand EditContractCommand
        {
            get
            {
                return _editContractCommand ??= new RelayCommand((selectedItem) =>
                {
                    //if (selectedItem == null) return;
                    Contract contract = selectedItem as Contract;
                    Contract con = new Contract()
                    {
                        Id = contract.Id,
                        AverageThroughput = contract.AverageThroughput,
                        MaxCarCount = contract.MaxCarCount,
                        RegularityOfDeliveries = contract.RegularityOfDeliveries,
                        CompanyName = contract.CompanyName
                    };
                    ContractWindow contractWindow = new ContractWindow(con);
                    if (contractWindow.ShowDialog() == true)
                    {
                        contract = _dbContext.Contracts.Find(contractWindow.Contract.Id);
                        if (contract != null)
                        {
                            if (!string.IsNullOrEmpty(contractWindow.Contract.CompanyName))
                            {
                                contract.AverageThroughput = contractWindow.Contract.AverageThroughput;
                                contract.MaxCarCount = contractWindow.Contract.MaxCarCount;
                                contract.RegularityOfDeliveries = contractWindow.Contract.RegularityOfDeliveries;
                                contract.CompanyName = contractWindow.Contract.CompanyName;
                                try
                                {
                                    _dbContext.Entry(contract).State = EntityState.Modified;
                                    _dbContext.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Trace.WriteLine(e);
                                    throw;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Заполните все поля!",
                                    "Ошибка!",
                                    MessageBoxButton.OK);
                            }
                        }
                    }
                });
            }
        }
        public RelayCommand DeleteContractCommand
        {
            get
            {
                return _deleteContractCommand ??= new RelayCommand((selectedItem) =>
                {
                    var res = MessageBox.Show("Вы хотите удалить запись?",
                                    "Внимание!",
                                    MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (selectedItem == null) return;
                        Contract contract = selectedItem as Contract;
                        try
                        {
                            _dbContext.Contracts.Remove(contract);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        if (selectedItem == null) return;
                    }
                });
            }
        }
        #endregion

        #region Terminal RelayCommands
        public RelayCommand AddTerminalCommand
        {
            get
            {
                return _addTerminalCommand ??= new RelayCommand((o) =>
                {
                    TerminalWindow terminalWindow = new TerminalWindow(new Terminal());
                    if (terminalWindow.ShowDialog() == true)
                    {
                        Terminal terminal = terminalWindow.Terminal;
                        _dbContext.Terminals.Add(terminal);
                        try
                        {
                            _dbContext.SaveChanges();
                        }
                        catch
                        {
                            _dbContext.Terminals.Remove(terminal);
                            MessageBox.Show("Заполните все поля!",
                                "Ошибка!",
                                MessageBoxButton.OK);
                        }
                    }
                });
            }
        }
        public RelayCommand EditTerminalCommand
        {
            get
            {
                return _editTerminalCommand ??= new RelayCommand((selectedItem) =>
                {
                    //if (selectedItem == null) return;
                    Terminal terminal = selectedItem as Terminal;
                    Terminal ter = new Terminal()
                    {
                        Id = terminal.Id,
                        CountProduct = terminal.CountProduct,
                        CountBelt = terminal.CountBelt,
                        CountGates = terminal.CountGates,
                        CountLoader = terminal.CountLoader
                    };
                    TerminalWindow terminalWindow = new TerminalWindow(ter);
                    if (terminalWindow.ShowDialog() == true)
                    {
                        terminal = _dbContext.Terminals.Find(terminalWindow.Terminal.Id);
                        if (terminal != null)
                        {
                            try
                            {
                                terminal.CountProduct = terminalWindow.Terminal.CountProduct;
                                terminal.CountBelt = terminalWindow.Terminal.CountBelt;
                                terminal.CountGates = terminalWindow.Terminal.CountGates;
                                terminal.CountLoader = terminalWindow.Terminal.CountLoader;
                                try
                                {
                                    _dbContext.Entry(terminal).State = EntityState.Modified;
                                    _dbContext.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Trace.WriteLine(e);
                                    throw;
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Заполните все поля!",
                                    "Ошибка!",
                                    MessageBoxButton.OK);
                                throw;
                            }
                        }
                    }
                });
            }
        }
        public RelayCommand DeleteTerminalCommand
        {
            get
            {
                return _deleteTerminalCommand ??= new RelayCommand((selectedItem) =>
                {
                    var res = MessageBox.Show("Вы хотите удалить запись?",
                                    "Внимание!",
                                    MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (selectedItem == null) return;
                        Terminal terminal = selectedItem as Terminal;
                        try
                        {
                            _dbContext.Terminals.Remove(terminal);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        if (selectedItem == null) return;
                    }
                });
            }
        }
        #endregion

        #region TypeTerminal RelayCommands
        public RelayCommand AddTypeTerminalCommand
        {
            get
            {
                return _addTypeTerminalCommand ??= new RelayCommand((o) =>
                {
                    TypeTerminalWindow typeTerminalWindow = new TypeTerminalWindow(new TypeTerminal());
                    if (typeTerminalWindow.ShowDialog() == true)
                    {
                        TypeTerminal typeTerminal = typeTerminalWindow.TypeTerminal;
                        _dbContext.TypeTerminals.Add(typeTerminal);
                        try
                        {
                            _dbContext.SaveChanges();
                        }
                        catch
                        {
                            _dbContext.TypeTerminals.Remove(typeTerminal);
                            MessageBox.Show("Заполните все поля!",
                                "Ошибка!",
                                MessageBoxButton.OK);
                        }
                    }
                });
            }
        }
        public RelayCommand EditTypeTerminalCommand
        {
            get
            {
                return _editTypeTerminalCommand ??= new RelayCommand((selectedItem) =>
                {
                    //if (selectedItem == null) return;
                    TypeTerminal typeTerminal = selectedItem as TypeTerminal;
                    TypeTerminal terminal = new TypeTerminal()
                    {
                        Id = typeTerminal.Id,
                        Name = typeTerminal.Name,
                        MinGates=  typeTerminal.MinGates,
                        MaxGates = typeTerminal.MaxGates,
                        FormTerminal = typeTerminal.FormTerminal
                    };
                    TypeTerminalWindow terminalWindow = new TypeTerminalWindow(terminal);
                    if (terminalWindow.ShowDialog() == true)
                    {
                        typeTerminal = _dbContext.TypeTerminals.Find(terminalWindow.TypeTerminal.Id);
                        if (typeTerminal != null)
                        {
                            try
                            {
                                typeTerminal.Name = terminalWindow.TypeTerminal.Name;
                                typeTerminal.MinGates = terminalWindow.TypeTerminal.MinGates;
                                typeTerminal.MaxGates = terminalWindow.TypeTerminal.MaxGates;
                                typeTerminal.FormTerminal = terminalWindow.TypeTerminal.FormTerminal;
                                try
                                {
                                    _dbContext.Entry(typeTerminal).State = EntityState.Modified;
                                    _dbContext.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Trace.WriteLine(e);
                                    throw;
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Заполните все поля!",
                                    "Ошибка!",
                                    MessageBoxButton.OK);
                                throw;
                            }
                        }
                    }
                });
            }
        }
        public RelayCommand DeleteTypeTerminalCommand
        {
            get
            {
                return _deleteTypeTerminalCommand ??= new RelayCommand((selectedItem) =>
                {
                    var res = MessageBox.Show("Вы хотите удалить запись?",
                                    "Внимание!",
                                    MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (selectedItem == null) return;
                        TypeTerminal typeTerminal = selectedItem as TypeTerminal;
                        try
                        {               
                            _dbContext.TypeTerminals.Remove(typeTerminal);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        if (selectedItem == null) return;
                    }
                });
            }
        }
        #endregion

        #region User RelayCommands
        public RelayCommand AddUserCommand
        {
            get
            {
                return _addUserCommand ??= new RelayCommand((o) =>
                {
                    UserWindow userWindow = new UserWindow(new User());
                    if (userWindow.ShowDialog() == true)
                    {
                        User user = userWindow.User;
                        _dbContext.Users.Add(user);
                        try
                        {
                            _dbContext.SaveChanges();
                        }
                        catch
                        {
                            _dbContext.Users.Remove(user);
                            MessageBox.Show("Заполните все поля!",
                                "Ошибка!",
                                MessageBoxButton.OK);
                        }
                    }
                });
            }
        }
        public RelayCommand EditUserCommand
        {
            get
            {
                return _editUserCommand ??= new RelayCommand((selectedItem) =>
                {
                    //if (selectedItem == null) return;
                    User user = selectedItem as User;
                    User u = new User()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Password = user.Password,
                        Role = user.Role 
                    };
                    UserWindow userWindow = new UserWindow(u);
                    if (userWindow.ShowDialog() == true) 
                    {
                        user = _dbContext.Users.Find(userWindow.User.Id);
                        if (user != null)
                        {
                            try
                            {
                                user.Username = userWindow.User.Username;
                                user.Password = userWindow.User.Password;
                                user.Role = userWindow.User.Role;
                                try
                                {
                                    _dbContext.Entry(user).State = EntityState.Modified;
                                    _dbContext.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Trace.WriteLine(e);
                                    throw;
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Заполните все поля!",
                                    "Ошибка!",
                                    MessageBoxButton.OK);
                                throw;
                            }
                        }
                    }
                });
            }
        }
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return _deleteUserCommand ??= new RelayCommand((selectedItem) =>
                {
                    var res = MessageBox.Show("Вы хотите удалить запись?",
                                    "Внимание!",
                                    MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (selectedItem == null) return;
                        User user = selectedItem as User;
                        try
                        {
                            _dbContext.Users.Remove(user);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        if (selectedItem == null) return;
                    }
                });
            }
        }
        #endregion
    }
}
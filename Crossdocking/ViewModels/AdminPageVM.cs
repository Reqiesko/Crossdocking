using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Crossdocking.Views;
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

        public object CurrentContext { get; set; }

        private RelayCommand _addCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _editCommand;

        public AdminPageVM()
        {
            _dbContext = new AppDbContext();
            
            try
            {
                _dbContext.Contracts.Load();
                _dbContext.Terminals.Load();
                _dbContext.TypeTerminals.Load();

                Contracts = _dbContext.Contracts.Local.ToObservableCollection();
                Terminals = _dbContext.Terminals.Local.ToObservableCollection();
                TypeTerminals = _dbContext.TypeTerminals.Local.ToObservableCollection();
                CurrentContext = Contracts;
            }
            catch { }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??= new RelayCommand((o) =>
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
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ??= new RelayCommand((selectedItem) =>
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
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand((selectedItem) =>
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
    }
}


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Crossdocking.Services;
using Crossdocking.Views.ViewWindows;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;


namespace Crossdocking.ViewModels
{
    public class ComputePageVM : BaseVM.ViewModelBase
    {
        private readonly AppDbContext _dbContext;
        public ObservableCollection<Terminal> Terminals { get; set; }
        public ObservableCollection<Contract> Contracts { get; set; }
        public ObservableCollection<TypeTerminal> TypeTerminals { get; set; }


        private readonly NavigationService _navigationService;


        private RelayCommand _showInformationCommand;
        private RelayCommand _backToLoginPageCommand;
        private RelayCommand _downloadFromFileCommand;
        private RelayCommand _executePlanningCommand;
        private ExcelParserService _excelParserService;


        public int CarsCount { get; set; }
        public int FastProductCount { get; set; }
        public int SlowProductCount { get; set; }
        public int DaysWithDeliveries { get; set; }
        public int AverageThroughput { get; set; }
        public double Inaccuracy { get; set; }
        public bool RegularityDeliveries { get; set; }
        public int ProductCount { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string[] RelativePathImages { get; set; }
        public string? AppDir { get; set; }
        
        private Dictionary<DateTime, int> DeliveriesInDay { get; set; }

        public ComputePageVM(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _dbContext = new AppDbContext();

            _dbContext.Terminals.Load();
            _dbContext.Contracts.Load();
            _dbContext.TypeTerminals.Load();

            Terminals = _dbContext.Terminals.Local.ToObservableCollection();
            Contracts = _dbContext.Contracts.Local.ToObservableCollection();
            TypeTerminals = _dbContext.TypeTerminals.Local.ToObservableCollection();

            AppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            RelativePathImages = new string[]
            {
                @"Images\I.png",
                @"Images\T.png",
                @"Images\L.png",
                @"Images\H.png",
                @"Images\U.png"
            };
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
        
        public RelayCommand DownloadFromFileCommand
        {
            get
            {
                return _downloadFromFileCommand ??= new RelayCommand(o =>
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    if (openFile.ShowDialog() == true)
                    {
                        _excelParserService = new ExcelParserService(openFile.FileName);
                        _excelParserService.ParseExcelFile();
                        GetMaxCarsCount();
                        GetAverageThroughput();
                        GetCategoriesProductCount();
                    }
                });
            }
        }
        
        public RelayCommand ExecutePlanningCommand
        {
            get
            {
                return _executePlanningCommand ??= new RelayCommand(o =>
                {
                    if (string.IsNullOrEmpty(CompanyName))
                    {
                        MessageBox.Show("Укажите название компании заказчика!",
                            "Предупреждение!",
                            MessageBoxButton.OK);
                        return;
                    }
                    try
                    {
                        if (_excelParserService.ProductTypes["Нескоропортящиеся"] < _excelParserService.ProductTypes["Скоропортящееся"])
                        {
                            if (IsRegularityDeliveries())
                            {
                                for (int i = 0; i < TypeTerminals.Count; i++)
                                {
                                    if (CarsCount >= TypeTerminals[i].MinGates && CarsCount <= TypeTerminals[i].MaxGates/* && ProductCount < Terminals[i].CountProduct*/)
                                    {
                                        ResultWindow resultWindow = new ResultWindow(
                                            Path.Combine(AppDir, RelativePathImages[i]),
                                            Terminals[i].CountLoader,
                                            Terminals[i].CountBelt,
                                            Terminals[i].CountGates);
                                        resultWindow.Show();
                                    }
                                }

                                Contract contract = new Contract()
                                {
                                    AverageThroughput = AverageThroughput,
                                    MaxCarCount = CarsCount,
                                    RegularityOfDeliveries = DaysWithDeliveries,
                                    CompanyName = CompanyName
                                };
                                _dbContext.Contracts.Add(contract);
                                try
                                {
                                    _dbContext.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    _dbContext.Contracts.Remove(contract);
                                    Trace.WriteLine(e);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Поставки нерегулярны!\nСквозное складирование не имеет смысла!",
                                    "Предупреждение!",
                                    MessageBoxButton.OK);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Преобладают нескоропортящееся товары!\nСквозное складирование не имеет смысла!",
                                "Предупреждение!",
                                MessageBoxButton.OK);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Сначала загрузите данные из файла!",
                            "Предупреждение!",
                            MessageBoxButton.OK);
                    }
                });
            }
        }

        private bool IsRegularityDeliveries()
        {
            double average = Convert.ToDouble(DeliveriesInDay.Values.Sum() 
                / (double)_excelParserService.Date.Distinct().ToList().Count);
            //var inaccuracy = 5;
            foreach (var day in DeliveriesInDay)
            {
                if (Math.Abs(day.Value - average) > Inaccuracy)
                {
                    return false;
                }
            }
            return true;
        } 
        private void GetMaxCarsCount()
        {
            DeliveriesInDay = new Dictionary<DateTime, int>();
            for (int i = 0; i < _excelParserService.Date.Count; i++)
            {
                if (DeliveriesInDay.TryAdd(_excelParserService.Date[i], 1))
                {
                }
                else
                {
                    DeliveriesInDay[_excelParserService.Date[i]]++;
                }
            }

            DaysWithDeliveries = DeliveriesInDay.Count;
            CarsCount = DeliveriesInDay.Values.Max();
            Inaccuracy = Convert.ToDouble((DeliveriesInDay.Values.Max() + DeliveriesInDay.Values.Min()) / 2);
        }
        
        private void GetAverageThroughput()
        {
            AverageThroughput = Convert.ToInt32(_excelParserService.Weight.Average());
        }

        private void GetCategoriesProductCount()
        {
            FastProductCount = _excelParserService.ProductCategories.Count - _excelParserService.ProductTypes["Нескоропортящиеся"];
            SlowProductCount = _excelParserService.ProductTypes["Нескоропортящиеся"];
            ProductCount = _excelParserService.ProductCategories.Count;
        }
    }
}

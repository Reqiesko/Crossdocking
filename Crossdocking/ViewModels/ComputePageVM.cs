using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossdocking.Services;
using DAL;
using Microsoft.Win32;


namespace Crossdocking.ViewModels
{
    public class ComputePageVM : BaseVM.ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        private readonly NavigationService _navigationService;

        private RelayCommand _showInformationCommand;
        private RelayCommand _backToLoginPageCommand;
        private RelayCommand _downloadFromFileCommand;
        private RelayCommand _executePlanningCommand;
        private ExcelParserService _excelParserService;

        public int CarsCount { get; set; }
        public int AverageThroughput { get; set; }
        public bool RegularityOfDeliveries { get; set; }
        public int LoaderCount { get; set; }
        public int BeltCount { get; set; }
        public int ProductCount { get; set; }
        public string CompanyName { get; set; }

        public ComputePageVM(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _dbContext = new AppDbContext();
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
                    openFile.ShowDialog();
                    _excelParserService = new ExcelParserService(openFile.FileName);
                    _excelParserService.ParseExcelFile();
                });
            }
        }
        
        public RelayCommand ExecutePlanningCommand
        {
            get
            {
                return _executePlanningCommand ??= new RelayCommand(o =>
                {
                    
                });
            }
        }
    }
}

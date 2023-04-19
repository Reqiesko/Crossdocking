using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Crossdocking.Commands;
using Crossdocking.Services;
using DAL;


namespace Crossdocking.ViewModels
{
    public class LoginPageVM : BaseVM.ViewModelBase
    {
        private readonly UsersDbContext _dbContext;

        private readonly NavigationService _navigationService;

        private ChangeWindowSizeService _changeWindowSizeService;

        private RelayCommand _loginCommand;

        private string _username;
        public string Username 
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private  string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage { get; set; }
        public LoginPageVM(NavigationService navigationService, ChangeWindowSizeService changeWindowSizeService)
        {
            _navigationService = navigationService;
            _dbContext = new UsersDbContext();
            _changeWindowSizeService = changeWindowSizeService;
        }

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??= new RelayCommand(o =>
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
                    if (user != null)
                    {
                        // Пользователь найден, выполняем необходимые действия, например, переходим на главную страницу приложения.
                        _changeWindowSizeService.ChangeSize(600, 600);
                        _navigationService.CurrentViewModel = new ComputePageVM();
                    }
                    else
                    {
                        ErrorMessage = "Fuck!";
                        // Пользователь не найден, выводим сообщение об ошибке.
                    }
                });
            }
        }
    }
}

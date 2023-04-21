using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Crossdocking.Services;
using DAL;


namespace Crossdocking.ViewModels
{
    public class LoginPageVM : BaseVM.ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        private readonly NavigationService _navigationService;

        private readonly ChangeWindowSizeService _changeWindowSizeService;

        private RelayCommand _loginCommand;

        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public LoginPageVM(NavigationService navigationService, ChangeWindowSizeService changeWindowSizeService)
        {
            _navigationService = navigationService;
            _dbContext = new AppDbContext();
            _changeWindowSizeService = changeWindowSizeService;
        }

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??= new RelayCommand(o =>
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
                    if (user is { Role: "user" })
                    {
                        // Пользователь найден, выполняем необходимые действия, например, переходим на главную страницу приложения.
                        _changeWindowSizeService.ChangeSize(600, 600);
                        _navigationService.CurrentViewModel = new ComputePageVM(_navigationService);
                    }
                    else if (user is { Role: "admin" })
                    {
                        _navigationService.CurrentViewModel = new AdminPageVM();
                    }
                    else
                    {
                        // Пользователь не найден, выводим сообщение об ошибке.
                        ErrorMessage = "Error";
                    }
                });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossdocking.Services;
using Crossdocking.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Crossdocking.Commands
{
    public class NavigateCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private MainViewModel _mvm;
        public NavigateCommand(NavigationService navigationService, MainViewModel mvm)
        {
            _mvm = mvm;
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.CurrentViewModel = _mvm;
        }
    }
}

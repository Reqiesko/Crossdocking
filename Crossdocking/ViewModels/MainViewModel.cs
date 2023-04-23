using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BaseVM;
using Crossdocking.Services;

namespace Crossdocking.ViewModels
{
    public class MainViewModel : BaseVM.ViewModelBase
    {
        private readonly NavigationService _navigationService;

        public ViewModelBase CurrentViewModel => _navigationService.CurrentViewModel;

        public MainViewModel(NavigationService service)
        {
            _navigationService = service;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}

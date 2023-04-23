using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace Crossdocking.ViewModels
{
    public class AdminPageVM : BaseVM.ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        private IEnumerable<Contract> _contracts;
        private IEnumerable<Terminal> _terminals;
        private IEnumerable<TypeTerminal> _typeTerminals;

        public IEnumerable<Contract> Contracts { get; set; }
        public IEnumerable<Terminal> Terminals { get; set; }
        public IEnumerable<TypeTerminal> TypeTerminals { get; set; }

        public AdminPageVM()
        {
            _dbContext = new AppDbContext();
            _dbContext.Contracts.Load();
            _dbContext.Terminals.Load();
            _dbContext.TypeTerminals.Load();

            Contracts = _dbContext.Contracts.Local.ToBindingList();
            Terminals = _dbContext.Terminals.Local.ToBindingList();
            TypeTerminals = _dbContext.TypeTerminals.Local.ToBindingList();
        }


    }
}


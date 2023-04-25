using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Contract : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int AverageThroughput { get; set; }
        public int MaxCarCount { get; set; }
        public int RegularityOfDeliveries { get; set; }
        public string CompanyName { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

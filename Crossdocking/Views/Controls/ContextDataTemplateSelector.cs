using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DAL;

namespace Crossdocking.Views.Controls
{
    public class ContextDataTemplateSelector : DataTemplateSelector 
    {
        public DataTemplate ContractTemplate { get; set; }
        public DataTemplate TerminalTemplate { get; set; }
        public DataTemplate TypeTerminalTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Contract)
            {
                return ContractTemplate;
            }
            else if (item is Terminal)
            {
                return TerminalTemplate;
            }
            else if (item is TypeTerminal)
            {
                return TypeTerminalTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}

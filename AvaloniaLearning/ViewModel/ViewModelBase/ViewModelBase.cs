using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaLearning.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        public void Initialize<T>(T @params)
        {
            InitializeParams(@params);
        }

        protected virtual void InitializeParams<T>(T @params) { }

        protected T GetType<T>(object? @params)
        {
            if (@params is T required)
                return required;
            else
                throw new ArgumentException("The passed type does not match the expected one");
        }
    }
}

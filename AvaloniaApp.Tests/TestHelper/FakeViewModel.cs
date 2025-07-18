using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApp.ViewModel;

namespace AvaloniaApp.Tests.TestHelper
{
    public class FakeViewModel : ViewModelBase
    {
        public (int, string) itemParam;

        protected override void InitializeParams<T>(T @params)
        {
            itemParam = GetType<(int, string)>(@params);
        }
    }
}

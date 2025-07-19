using AvaloniaApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApp.Tests.TestHelper
{
    public class FakeViewModel2 : ViewModelBase
    {
        public Animal animal { get; set; } = new Animal();

        protected override void InitializeParams<T>(T @params)
        {
            animal = GetAs<Animal>(@params);
        }
    }
}

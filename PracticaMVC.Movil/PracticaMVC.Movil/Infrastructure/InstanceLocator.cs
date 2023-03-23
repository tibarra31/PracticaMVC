using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaMVC.Movil.Infrastructure
{
    using ViewModels;

    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}

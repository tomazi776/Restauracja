using Prism.Events;
using Restauracja.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Restauracja
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private IEventAggregator ea;
        public App()
        {
            InitializeComponent();

            //this.ea = new EventAggregator();

            //UnityContainer unityContainer = new UnityContainer();
            //unityContainer.RegisterType<IEventAggregator, EventAggregatorService>();
            //ServiceLocator.SetLocatorProvider(() => new Utilities.ServiceLocator());

            //MainWindow = new MainWindow(ea);
        }




    }
}

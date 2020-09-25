﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restauracja.Services
{
    public class WindowService : IWindowService
    {
        public void ShowWindow(object viewModel)
        {
            var win = new Window();
            win.Content = viewModel;
            win.Show();
        }

        //public void ShowWindow(object viewModel, params object[] )
        //{
        //    var win = new Window();
        //    win.Content = viewModel;
        //    win.Show();
        //}
    }
}

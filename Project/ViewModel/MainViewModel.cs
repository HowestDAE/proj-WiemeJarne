using CommunityToolkit.Mvvm.ComponentModel;
using Project.Model;
using Project.Repository;
using Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public Page CurrentPage { get; set; }

        public OverviewPage OverviewPage { get; }
        = new OverviewPage();

        public DetailPage DetailPage { get; }
        = new DetailPage();
         
        public MainViewModel()
        {
            CurrentPage = OverviewPage;
        }
    }
}
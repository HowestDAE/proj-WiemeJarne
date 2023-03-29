using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Model;
using Project.Repository;
using Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public string CommandText
        {
            get
            {
                if (CurrentPage is OverviewPage)
                    return "SHOW DEALS";

                return "BACK";
            }
        }

        public RelayCommand SwitchPageCommand { get; private set; }

        public Page CurrentPage { get; set; }

        public OverviewPage OverviewPage { get; }
        = new OverviewPage();

        public DetailPage DetailPage { get; }
        = new DetailPage();
        public MainViewModel()
        {
            CurrentPage = OverviewPage;
            SwitchPageCommand = new RelayCommand(SwitchPage);
        }

        public void SwitchPage()
        {
            if (CurrentPage is OverviewPage)
            {
                Game selectedGame = (OverviewPage.DataContext as OverviewVM).SelectedGame;
                if (selectedGame == null) return;

                string selectedStoreName = (OverviewPage.DataContext as OverviewVM).SelectedStoreName;
                if (selectedStoreName == null) return;

                (DetailPage.DataContext as DetailVM).CurrentGame = selectedGame;
                (DetailPage.DataContext as DetailVM).SelectedStoreName = selectedStoreName;

                CurrentPage = DetailPage;
            }
            else
            {
                CurrentPage = OverviewPage;
            }

            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(CommandText));
        }
    }
}
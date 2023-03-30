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
        public RelayCommand SearchDealsCommand { get; private set; }

        public Page CurrentPage { get; set; }

        public OverviewPage OverviewPage { get; }
        = new OverviewPage();

        public DetailPage DetailPage { get; }
        = new DetailPage();

        public Visibility IsSearchButtonVisible { get; set; } = Visibility.Visible;

        public MainViewModel()
        {
            CurrentPage = OverviewPage;
            SwitchPageCommand = new RelayCommand(SwitchPage);
            SearchDealsCommand = new RelayCommand(SearchDeals);
        }

        public void SwitchPage()
        {
            if (CurrentPage is OverviewPage)
            {
                Game selectedGame = (OverviewPage.DataContext as OverviewVM).SelectedGame;
                if (selectedGame == null) return;

                Store selectedStore = (OverviewPage.DataContext as OverviewVM).SelectedStore;
                if (selectedStore == null) return;

                

                (DetailPage.DataContext as DetailVM).CurrentGame = selectedGame;
                (DetailPage.DataContext as DetailVM).SelectedStore = selectedStore;
                


                CurrentPage = DetailPage;
                IsSearchButtonVisible = Visibility.Hidden;
            }
            else
            {
                CurrentPage = OverviewPage;
                IsSearchButtonVisible = Visibility.Visible;
            }

            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(CommandText));
            OnPropertyChanged(nameof(IsSearchButtonVisible));
        }

        public void SearchDeals()
        {
            if (CurrentPage is OverviewPage)
            {
                Store selectedStore = (OverviewPage.DataContext as OverviewVM).SelectedStore;
                if (selectedStore == null) return;

                string selectedComparisonOperator = (OverviewPage.DataContext as OverviewVM).SelectedComparisonOperator;
                if (selectedComparisonOperator == null) return;

                string selectedComparisonType = (OverviewPage.DataContext as OverviewVM).SelectedComparisonType;
                if (selectedComparisonType == null) return;

                float givenToCompareNumber = (OverviewPage.DataContext as OverviewVM).GivenToCompareNumber;

                (DetailPage.DataContext as DetailVM).SelectedStore = selectedStore;
                (DetailPage.DataContext as DetailVM).SelectedComparisonOperator = selectedComparisonOperator;
                (DetailPage.DataContext as DetailVM).SelectedComparisonType = selectedComparisonType;
                (DetailPage.DataContext as DetailVM).GivenToCompareNumber = givenToCompareNumber;

                (OverviewPage.DataContext as OverviewVM).UpdateGames();
            }
        }
    }
}
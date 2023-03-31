using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Model;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.ViewModel
{
    public class OverviewVM : ObservableObject
    {
        private readonly bool _useApi = true;

        private APIGameRepository ApiGameRepository { get; set; }

        private List<Game> _games;
        public List<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged(nameof(Games));
            }
        }

        private Game _selectedGame;
        public Game SelectedGame
        {
            get { return _selectedGame; }
            set { _selectedGame = value; }
        }

        private List<Store> _stores;
        public List<Store> Stores
        {
            get { return _stores; }
            private set
            {
                _stores = value;
                OnPropertyChanged(nameof(Stores));
            }
        }

        public Store _selectedStore;
        public Store SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                _selectedStore = value;
                OnPropertyChanged(nameof(SelectedStore));
            }
        }

        public List<string> ComparisonOperators { get; private set; }
            = new List<string>()
            {
                ">",
                "<",
                "="
            };

        private string _selectedComparisonOperator;
        public string SelectedComparisonOperator
        {
            get { return _selectedComparisonOperator; }
            set
            {
                _selectedComparisonOperator = value;
                OnPropertyChanged(nameof(SelectedComparisonOperator));
            }
        }

        public List<string> Types { get; private set; }
            = new List<string>()
            {
                "USD",
                "%"
            };

        private string _selectedType;

        public string SelectedComparisonType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedComparisonType));
            }
        }

        private float givenToCompareNumber;
        public float GivenToCompareNumber
        {
            get { return givenToCompareNumber; }
            set
            {
                givenToCompareNumber = value;
                OnPropertyChanged(nameof(GivenToCompareNumber));
            }
        }

        public RelayCommand LoadGamesCommand { get; private set; }

        public OverviewVM()
        {
            if (_useApi)
            {
                ApiGameRepository = new APIGameRepository();

                LoadGames(100);
                LoadStores();
            }
            else
            {
                Games = LocalGameRepository.GetGames();
                Stores = LocalGameRepository.GetStores();
            }

            LoadGamesCommand = new RelayCommand(Load100Games);
        }

        private async void LoadGames(int minAmount)
        {
            Games = await ApiGameRepository.LoadGamesAsync(minAmount);
        }

        private async void Load100Games()
        {
            Games = await ApiGameRepository.LoadGamesAsync(100);
            UpdateGames();
        }

        private async void LoadStores()
        {
            Stores = await ApiGameRepository.GetStores();
            Stores.Add(new Store() { Name = "<all stores>", Id = "" });
            SelectedStore = Stores.Last();
            SelectedComparisonOperator = ComparisonOperators[0];
            SelectedComparisonType = Types[0];
            GivenToCompareNumber = 0.00f;
        }

        public async void UpdateGames()
        {
            if (_useApi)
                Games = await ApiGameRepository.GetGamesAsync(SelectedStore.Name, SelectedComparisonOperator, SelectedComparisonType, GivenToCompareNumber);
            else
                Games = LocalGameRepository.GetGames(SelectedStore.Name, SelectedComparisonOperator, SelectedComparisonType, GivenToCompareNumber);
        }
    }
}

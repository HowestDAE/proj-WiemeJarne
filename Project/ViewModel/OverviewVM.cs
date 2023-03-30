using CommunityToolkit.Mvvm.ComponentModel;
using Project.Model;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.ViewModel
{
    public class OverviewVM : ObservableObject
    {
        public List<Game> Games { get; set; }

        private Game _selectedGame;
        public Game SelectedGame
        {
            get { return _selectedGame; }
            set { _selectedGame = value; }
        }

        public List<string> StoreNames { get; private set; }
        public List<Store> Stores { get; private set; }

        public Store _selectedStore;
        public Store SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                _selectedStore = value;
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
            }
        }

        private float givenToCompareNumber;
        public float GivenToCompareNumber
        {
            get { return givenToCompareNumber; }
            set
            {
                givenToCompareNumber = value;
            }
        }

        public OverviewVM()
        {
            Games = LocalGameRepository.GetGames();
            Stores = LocalGameRepository.GetStores();

            Stores.Add(new Store() { Name = "<all stores>", Id = "" });

            SelectedStore = Stores.Last();
            SelectedComparisonOperator = ComparisonOperators[0];
            SelectedComparisonType = Types[0];
            GivenToCompareNumber = 0.00f;
        }

        public void UpdateGames()
        {
            Games = LocalGameRepository.GetGames(SelectedStore.Name, SelectedComparisonOperator, SelectedComparisonType, GivenToCompareNumber);
            OnPropertyChanged(nameof(Games));
        }
    }
}

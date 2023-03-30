using CommunityToolkit.Mvvm.ComponentModel;
using Project.Model;
using Project.Repository;
using System;
using System.Collections.Generic;

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

<<<<<<< HEAD
        {
            set
            {
=======
        public string _selectedStoreName;
        public string SelectedStoreName
        {
            get { return _selectedStoreName; }
            set
            {
                _selectedStoreName = value;
>>>>>>> parent of 7087c68 (changed the comboBox for the stores to also have the icon in it)
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
<<<<<<< HEAD
            Stores.Add(new Store() { Name = "<all stores>", Id = "" });
            SelectedStore = Stores.Last();
=======
            StoreNames = LocalGameRepository.GetStoreNames();
            StoreNames.Add("<all stores>");
            SelectedStoreName = "<all stores>";
>>>>>>> parent of 7087c68 (changed the comboBox for the stores to also have the icon in it)
            SelectedComparisonOperator = ComparisonOperators[0];
            SelectedComparisonType = Types[0];
            GivenToCompareNumber = 0.00f;
        }

        public void UpdateGames()
        {
<<<<<<< HEAD
=======
            Games = LocalGameRepository.GetGames(SelectedStoreName, SelectedComparisonOperator, SelectedComparisonType, GivenToCompareNumber);
>>>>>>> parent of 7087c68 (changed the comboBox for the stores to also have the icon in it)
            OnPropertyChanged(nameof(Games));
        }
    }
}

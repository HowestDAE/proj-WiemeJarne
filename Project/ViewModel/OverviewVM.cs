using CommunityToolkit.Mvvm.ComponentModel;
using Project.Model;
using Project.Repository;
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

        public string _selectedStoreName;
        public string SelectedStoreName
        {
            get { return _selectedStoreName; }
            set 
            { 
                _selectedStoreName = value;

                if(SelectedStoreName.Equals("<all stores>"))
                {
                    Games = LocalGameRepository.GetGames("-1"); //if all the stores should be shown use index of -1
                }
                else
                {
                    foreach (var store in Stores)
                    {
                        if (SelectedStoreName.Equals(store.Name))
                        {
                            Games = LocalGameRepository.GetGames(store.Id);
                            break;
                        }
                    }
                }

                OnPropertyChanged(nameof(Games));
            }
        }

        public List<string> Sorters { get; private set; }
        = new List<string>()
        {
            "A-Z",
            "Z-A"
        };

        public OverviewVM()
        {
            Games = LocalGameRepository.GetGames();
            Stores = LocalGameRepository.GetStores();
            StoreNames = LocalGameRepository.GetStoreNames();
            StoreNames.Add("<all stores>");
            SelectedStoreName = "<all stores>";
        }
    }
}

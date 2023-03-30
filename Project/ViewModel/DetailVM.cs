using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Model;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModel
{
    public class DetailVM : ObservableObject
    {
        private Game _currentGame;
            //= new Game()
            //{
            //    Title = "minecraft",
            //    ImageUrl = "https://hb.imgix.net/9524a09e38dc390586719f5803df22a8d608c29d.jpeg?auto=compress,format&fit=crop&h=84&w=135&s=8567b12022081cb16d3462ab09d5b85c",
            //    Deals = new List<Deal>()
            //    {
            //        new Deal()
            //        {
            //            StoreId = "1",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "2",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "3",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "4",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "5",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "6",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "7",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "8",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "9",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        },
            //        new Deal()
            //        {
            //            StoreId = "10",
            //            SalePrice = 0,
            //            NormalPrice = 5.0f,
            //            SavingPercentage = 100f,
            //        }
            //    }
            //};

        public Game CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;
                OnPropertyChanged(nameof(CurrentGame));
            }
        }

        private string _userEmail = "(enter email here)";
        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                _userEmail = value;
                OnPropertyChanged(nameof(UserEmail));
            }
        }

        private string _priceToReach = "(enter price to reach here)";
        public string PriceToReach
        {
            get { return _priceToReach; }
            set
            {
                _priceToReach = value;
                OnPropertyChanged(nameof(PriceToReach));
            }
        }

        public RelayCommand BrowseToSelectedDealCommand { get; private set; }
        public RelayCommand SetAlertCommand { get; private set; }

        private Deal _selectedDeal;
        public Deal SelectedDeal
        {
            get { return _selectedDeal; }
            set
            {
                _selectedDeal = value;
            }
        }

        public List<Store> Stores { get; private set; }

        public List<string> StoreNames { get; private set; }

        public Store _selectedStore;
        public Store SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                _selectedStore = value;
                CalculateShowingDeals();
            }
        }

        private string selectedComparisonOperator;
        public string SelectedComparisonOperator
        {
            get { return selectedComparisonOperator; }
            set
            {
                selectedComparisonOperator = value;
                CalculateShowingDeals();
            }
        }

        private string selectedComparisonType;
        public string SelectedComparisonType
        {
            get { return selectedComparisonType; }
            set
            {
                selectedComparisonType = value;
                CalculateShowingDeals();
            }
        }

        private float givenToCompareNumber;
        public float GivenToCompareNumber
        {
            get { return givenToCompareNumber; }
            set
            {
                givenToCompareNumber = value;
                CalculateShowingDeals();
            }
        }

        private List<Deal> _showingDeals;
        public List<Deal> ShowingDeals
        {
            get { return _showingDeals; }
            set
            {
                _showingDeals = value;
                OnPropertyChanged(nameof(ShowingDeals));
            }
        }
        public DetailVM()
        {
            BrowseToSelectedDealCommand = new RelayCommand(BrowseToSelectedDeal);
            SetAlertCommand = new RelayCommand(SetAlert);
            Stores = LocalGameRepository.GetStores();
            StoreNames = LocalGameRepository.GetStoreNames();
            //ShowingDeals = CurrentGame.Deals;
        }

        public void CalculateShowingDeals()
        {
            List<Deal> showingDeals = new List<Deal>();
            //determine the store index of the SelectedStoreName
            string selectedStoreId = "";
            foreach (var store in Stores)
            {
                if (store.Name.Equals(SelectedStore.Name))
                {
                    selectedStoreId = store.Id;
                    break;
                }
            }

            if (SelectedComparisonOperator == null) return;
            if(SelectedComparisonType == null) return;

            foreach (var deal in CurrentGame.Deals)
            {
                if(LocalGameRepository.CheckDeal(deal, selectedStoreId, SelectedComparisonOperator, SelectedComparisonType, GivenToCompareNumber))
                    showingDeals.Add(deal);
            }

            ShowingDeals = showingDeals;
        }

        public void BrowseToSelectedDeal()
        {
            string url = $"https://www.cheapshark.com/redirect?dealID={SelectedDeal.DealId}";
            Process.Start(url);
        }

        public void SetAlert() //this function will only work when the api is in use
        {

        }
    }
}
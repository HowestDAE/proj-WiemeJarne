using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Model;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    public class DetailVM : ObservableObject
    {
        private readonly bool _useApi = true;

        private APIGameRepository ApiGameRepository { get; set; }

        private Game _currentGame;
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
        public RelayCommand SetPriceAlertCommand { get; private set; }
        public RelayCommand DeletePriceAlertCommand { get; private set; }

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

        private Store _selectedStore;
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
            SetPriceAlertCommand = new RelayCommand(SetPriceAlert);
            DeletePriceAlertCommand = new RelayCommand(DeletePriceAlert);
            if(_useApi)
            {
                ApiGameRepository = new APIGameRepository();
                LoadStores();
            }
            else
            {
                Stores = LocalGameRepository.GetStores();
            }
        }

        private async void LoadStores()
        {
            Stores = await ApiGameRepository.GetStores();
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
            if(CurrentGame == null) return;

            foreach (var deal in CurrentGame.Deals)
            {

                if (_useApi && LocalGameRepository.CheckDeal(deal, selectedStoreId, SelectedComparisonOperator, SelectedComparisonType, GivenToCompareNumber))
                    showingDeals.Add(deal);
                else if (!_useApi && ApiGameRepository.CheckDeal(deal, selectedStoreId, SelectedComparisonOperator, SelectedComparisonType, GivenToCompareNumber))
                    showingDeals.Add(deal);
            }

            ShowingDeals = showingDeals;
        }

        public void BrowseToSelectedDeal()
        {
            string url = $"https://www.cheapshark.com/redirect?dealID={SelectedDeal.DealId}";
            Process.Start(url);
        }

        public async void SetPriceAlert() //this function only work when the api is in use
        {
            if(!_useApi)
                MessageBox.Show("Failed set alert this feature is only available when using the api", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                await ApiGameRepository.SetPriceAlertAsync(UserEmail, CurrentGame.Id, PriceToReach);
            }
        }

        public async void DeletePriceAlert() //this function only works when the api is in use
        {
            if (!_useApi)
                MessageBox.Show("Failed delete alert this feature is only available when using the api", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                await ApiGameRepository.DeletePriceAlertAsync(UserEmail, CurrentGame.Id, PriceToReach);
            }
        }
    }
}
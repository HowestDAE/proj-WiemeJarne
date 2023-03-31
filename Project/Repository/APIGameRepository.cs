using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Repository
{
    public class APIGameRepository
    {
        private static List<Game> _games;
        private static int _latestGameId; //the id of the last game that was tried to load in
        public async Task<List<Game>> LoadGamesAsync(int minAmountOfGamesToLoad) //it is likely that a bit more games then the given number will be loaded in this is because the games are loaded in dictionary with a max 25 games per request
        {
            if (_games == null)
                _games = new List<Game>();

            //create the endPoint with 25 ids this is the max amount of games per request
            //the id from the last loaded game is used because some id's aren't used so it is not possible to use _games.Count()

            int amountOfNewlyLoadedGames = 0;
            while (amountOfNewlyLoadedGames < minAmountOfGamesToLoad)
            {
                string endPoint = CalculateGamesEndPoint();
                _latestGameId += 25;
                amountOfNewlyLoadedGames += await LoadGamesAsync(endPoint);
            }

            return _games;
        }

        private string CalculateGamesEndPoint()
        {
            string gameIds = $"{_latestGameId + 1},{_latestGameId + 2},{_latestGameId + 3},{_latestGameId + 4},{_latestGameId + 5},{_latestGameId + 6},{_latestGameId + 7},{_latestGameId + 8},{_latestGameId + 9},{_latestGameId + 10},{_latestGameId + 11},{_latestGameId + 12},{_latestGameId + 13},{_latestGameId + 14},{_latestGameId + 15},{_latestGameId + 16},{_latestGameId + 17},{_latestGameId + 18},{_latestGameId + 19},{_latestGameId + 20},{_latestGameId + 21},{_latestGameId + 22},{_latestGameId + 23},{_latestGameId + 24},{_latestGameId + 25}";
            return $"https://www.cheapshark.com/api/1.0/games?ids={gameIds}";
        }

        private async Task<int> LoadGamesAsync(string endPoint)
        {
            int amountOfNewlyLoadedGames = 0;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(endPoint);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.ReasonPhrase);

                    string json = await response.Content.ReadAsStringAsync();

                    var obj = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);

                    foreach (var item in obj)
                    {
                        Game game = new Game();
                        game.Id = Convert.ToInt32(item.Key);
                        var info = item.Value.SelectToken("info");
                        game.Title = info.SelectToken("title").ToString();
                        game.ImageUrl = info.SelectToken("thumb").ToString();
                        game.Deals = item.Value.SelectToken("deals").ToObject<List<Deal>>();
                        _games.Add(game);
                        ++amountOfNewlyLoadedGames;
                    }
                    return amountOfNewlyLoadedGames;
                }
                catch (HttpRequestException exception)
                {
                    MessageBox.Show($"{exception.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return 0;
            }
        }

        public async Task<List<Game>> GetGamesAsync(string storeName, string comparisonOperator, string comparisonType, float toCompareNumber)
        {
            bool shouldCheckStoreId = false;
            if (!storeName.Equals("<all stores>"))
                shouldCheckStoreId = true;

            List<Game> filteredGames = new List<Game>();

            string storeId = "";
            if (shouldCheckStoreId)
            {
                storeId = await GetStoreIdAsync(storeName);
            }

            if (comparisonOperator == null)
                return _games;

            if (comparisonType == null)
                return _games;

            //loop over all the games
            foreach (Game game in _games)
            {
                int amountOfFilteredDeals = 0;
                //loop over all the deal in from the games and if the game has a deal with the given storeName add it to the _gamesByStore list
                foreach (Deal deal in game.Deals)
                {
                    if (CheckDeal(deal, storeId, comparisonOperator, comparisonType, toCompareNumber))
                        ++amountOfFilteredDeals;
                }

                if (amountOfFilteredDeals > 0)
                    filteredGames.Add(game);
            }

            return filteredGames;
        }

        public bool CheckDeal(Deal deal, string storeId, string comparisonOperator, string comparisonType, float toCompareNumber)
        {
            bool shouldCheckStoreId = true;
            if (storeId.Equals(""))
                shouldCheckStoreId = false;

            if (shouldCheckStoreId && deal.StoreId.Equals(storeId))
            {
                if (comparisonType.Equals("USD") && CheckDealSalePrice(deal.SalePrice, comparisonOperator, toCompareNumber))
                    return true;
                else if (comparisonType.Equals("%") && CheckDealSalePercentage(deal.SavingPercentage, comparisonOperator, toCompareNumber))
                    return true;
            }
            else if (!shouldCheckStoreId)
            {
                if (comparisonType.Equals("USD") && CheckDealSalePrice(deal.SalePrice, comparisonOperator, toCompareNumber))
                    return true;
                else if (comparisonType.Equals("%") && CheckDealSalePercentage(deal.SavingPercentage, comparisonOperator, toCompareNumber))
                    return true;
            }

            return false;
        }

        private bool CheckDealSalePrice(float price, string comparisonOperator, float toCompareNumber)
        {
            if (comparisonOperator.Equals("<") && price < toCompareNumber)
                return true;

            if (comparisonOperator.Equals(">") && price > toCompareNumber)
                return true;

            if (comparisonOperator.Equals("=") && price == toCompareNumber)
                return true;

            return false;
        }

        private bool CheckDealSalePercentage(float pertencate, string comparisonOperator, float toCompareNumber)
        {
            if (comparisonOperator.Equals("<") && pertencate < toCompareNumber)
                return true;

            if (comparisonOperator.Equals(">") && pertencate > toCompareNumber)
                return true;

            if (comparisonOperator.Equals("=") && pertencate == toCompareNumber)
                return true;

            return false;
        }

        private static List<Store> _stores;
        public async Task<List<Store>> GetStores()
        {
            if (_stores == null)
                _stores = new List<Store>();
            else return _stores;

            string endPoint = "https://www.cheapshark.com/api/1.0/stores";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(endPoint);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.ReasonPhrase);

                    string json = await response.Content.ReadAsStringAsync();

                    var obj = JsonConvert.DeserializeObject<List<JObject>>(json);

                    foreach (var item in obj)
                    {
                        //if the store is inactive don't add it to the list
                        if (!item.SelectToken("isActive").ToObject<bool>())
                            continue;

                        Store store = new Store();
                        store.Id = item.SelectToken("storeID").ToString();
                        store.Name = item.SelectToken("storeName").ToString();
                        var images = item.SelectToken("images");
                        string imagesUrlStart = "https://www.cheapshark.com/";
                        store.BannerUrl = imagesUrlStart + images.SelectToken("banner").ToString();
                        store.LogoUrl = imagesUrlStart + images.SelectToken("logo").ToString();
                        store.IconUrl = imagesUrlStart + images.SelectToken("icon").ToString();
                        _stores.Add(store);
                    }

                    return _stores;
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Failed to load stores", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return null;
            }
        }

        private async Task<string> GetStoreIdAsync(string storeName)
        {
            await GetStores();

            string storeId = "";
            foreach (var store in _stores)
            {
                if (store.Name.Equals(storeName))
                {
                    storeId = store.Id;
                    break;
                }
            }

            return storeId;
        }

        public async Task SetPriceAlertAsync(string email, int gameId, string price)
        {
            string endPoint = $"https://www.cheapshark.com/api/1.0/alerts?action=set&email={email}&gameID={gameId}&price={price}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(endPoint);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.ReasonPhrase);

                    string json = await response.Content.ReadAsStringAsync();

                    string result = JsonConvert.DeserializeObject<string>(json);

                    MessageBox.Show($"price alert set succeeded you will receive an email from no-reply@cheapshark.com when the price drops below {price} USD", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Failed set alert please enter a valid e-mail address", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async Task DeletePriceAlertAsync(string email, int gameId, string price)
        {
            string endPoint = $"https://www.cheapshark.com/api/1.0/alerts?action=set&email={email}&gameID={gameId}&price={price}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(endPoint);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.ReasonPhrase);

                    string json = await response.Content.ReadAsStringAsync();

                    string result = JsonConvert.DeserializeObject<string>(json);

                    MessageBox.Show($"price alert delete succeeded you will no longer receive emails for this game", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Failed delete alert please enter a valid e-mail address", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Model;
using Project.View.Converters;

namespace Project.Repository
{
    public class LocalGameRepository
    {
        private static List<Game> _games;
        public static List<Game> GetGames()
        {
            if (_games != null) return _games;

            _games = new List<Game>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            var resourceName = "Project.Resources.Data.games.json";

            using(Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using(var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    
                    var obj = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);

                    foreach(var item in obj)
                    {
                        Game game = new Game();
                        var info = item.Value.SelectToken("info");
                        game.Title = info.SelectToken("title").ToString();
                        game.ImageUrl = info.SelectToken("thumb").ToString();
                        game.Deals = item.Value.SelectToken("deals").ToObject<List<Deal>>();
                        _games.Add(game);
                    }
                }
            }

            return _games;
        }

        public static List<Game> GetGames(string storeName, string comparisonOperator, string comparisonType, float toCompareNumber)
        {
            bool shouldCheckStoreId = false;
            if (!storeName.Equals("<all stores>"))
                shouldCheckStoreId = true;

            List<Game> filteredGames = new List<Game>();

            string storeId = "";
            if (shouldCheckStoreId)
            {
                storeId = GetStoreId(storeName);
            }

            if (comparisonOperator == null)
                return GetGames();

            if (comparisonType == null)
                return GetGames();

            //loop over all the games
            foreach(Game game in _games)
            {
                int amountOfFilteredDeals = 0;
                //loop over all the deal in from the games and if the game has a deal with the given storeName add it to the _gamesByStore list
                foreach(Deal deal in game.Deals)
                {
                    if (CheckDeal(deal, storeId, comparisonOperator, comparisonType, toCompareNumber))
                        ++amountOfFilteredDeals;
                }

                if(amountOfFilteredDeals > 0)
                    filteredGames.Add(game);
            }

            return filteredGames;
        }

        public static bool CheckDeal(Deal deal, string storeId, string comparisonOperator, string comparisonType, float toCompareNumber)
        {
            bool shouldCheckStoreId = true;
            if(storeId.Equals(""))
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

        private static bool CheckDealSalePrice(float price, string comparisonOperator, float toCompareNumber)
        {
            if (comparisonOperator.Equals("<") && price < toCompareNumber)
                return true;

            if (comparisonOperator.Equals(">") && price > toCompareNumber)
                return true;

            if (comparisonOperator.Equals("=") && price == toCompareNumber)
                return true;

            return false;
        }

        private static bool CheckDealSalePercentage(float pertencate, string comparisonOperator, float toCompareNumber)
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
        public static List<Store> GetStores()
        {
            if (_stores != null) return _stores;

            _stores = new List<Store>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            var resourceName = "Project.Resources.Data.stores.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();

                    var obj = JsonConvert.DeserializeObject<List<JObject>>(json);

                    foreach (var item in obj)
                    {
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
                }
            }

            return _stores;
        }

        private static string GetStoreId(string storeName)
        {
            GetStores();

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

        public static List<string> GetStoreNames()
        {
            List<string>storeNames = new List<string>();

            if (_stores == null) return storeNames;

            foreach (var item in _stores)
            {
                storeNames.Add(item.Name);
            }

            return storeNames;
        }
    }
}

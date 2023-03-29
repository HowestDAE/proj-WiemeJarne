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

        public static List<Game> GetGames(string storeId)
        {
            if (storeId.Equals("-1"))
                return GetGames();

            List<Game> gamesByStore = new List<Game>();

            //loop over all the games
            foreach(Game game in _games)
            {
                //loop over all the deal in from the games and if the game has a deal with the given storeName add it to the _gamesByStore list
                foreach(Deal deal in game.Deals)
                {
                    if (deal.StoreId.Equals(storeId))
                        gamesByStore.Add(game);
                }
            }

            return gamesByStore;
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
                        store.BannerUrl = images.SelectToken("banner").ToString();
                        store.LogoUrl = images.SelectToken("logo").ToString();
                        store.IconUrl = images.SelectToken("icon").ToString();
                        _stores.Add(store);
                    }
                }
            }

            return _stores;
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

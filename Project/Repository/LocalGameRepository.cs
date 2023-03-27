using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Model;

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

        public LocalGameRepository()
        {
            GetGames();
        }
    }
}

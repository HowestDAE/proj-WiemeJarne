using Project.Model;
using Project.Repository;
using System.Collections.Generic;

namespace Project.ViewModel
{
    public class OverviewVM
    {
        public List<Game> Games { get; set; }

        private Game _selectedGame;
        public Game SelectedGame
        {
            get { return _selectedGame; }
            set { _selectedGame = value; }
        }

        public OverviewVM()
        {
            Games = LocalGameRepository.GetGames();
        }
    }
}

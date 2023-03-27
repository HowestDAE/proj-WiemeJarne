using Project.Model;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class OverviewVM
    {
        public List<Game> Games { get; set; }

        public OverviewVM()
        {
            Games = LocalGameRepository.GetGames();
        }
    }
}

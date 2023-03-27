using CommunityToolkit.Mvvm.ComponentModel;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class DetailVM : ObservableObject
    {
        private Game _currentGame
            = new Game()
            {
                Title = "minecraft",
                ImageUrl = "https://hb.imgix.net/9524a09e38dc390586719f5803df22a8d608c29d.jpeg?auto=compress,format&fit=crop&h=84&w=135&s=8567b12022081cb16d3462ab09d5b85c",
                Deals = new List<Deal>()
                {
                    new Deal()
                    {
                        StoreId = "1",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "2",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "3",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "4",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "5",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "6",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "7",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "8",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "9",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    },
                    new Deal()
                    {
                        StoreId = "10",
                        SalePrice = 0,
                        NormalPrice = 5.0f,
                        SavingPercentage = 100f,
                    }
                }
            };

        public Game CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;
                OnPropertyChanged(nameof(CurrentGame));
            }
        }
    }
}
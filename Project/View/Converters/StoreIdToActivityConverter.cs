using Project.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Project.View.Converters
{
    internal class StoreIdToActivityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string id = value.ToString();
            var stores = LocalGameRepository.GetStores();
            foreach (var store in stores)
            {
                if (store.Id == id)
                    return store.IsActive;
            }

            return "invalid storeId";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

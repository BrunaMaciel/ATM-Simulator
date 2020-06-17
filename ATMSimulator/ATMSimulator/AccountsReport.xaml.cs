using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ATMLibrary;

namespace ATMSimulator
{
    /// <summary>
    /// Interaction logic for AccountsReport.xaml
    /// </summary>
    public partial class AccountsReport : Window
    {
        public AccountsReport(List<Account> accounts)
        {
            InitializeComponent();
            accountsList.ItemsSource = accounts;
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    [ValueConversion(typeof(string), typeof(Decimal))]
    class BalanceConverter : IValueConverter
    {

        object IValueConverter.Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
                return String.Format("{0:C}", value);
            else
                return "";
        }

        object IValueConverter.ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

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
    public partial class TransactionsReport : Window
    {
        public TransactionsReport(List<Transaction> transactions)
        {
            InitializeComponent();
            transactionsList.ItemsSource = transactions;
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

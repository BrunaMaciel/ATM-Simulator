using System;
using System.Collections.Generic;
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
using System.Globalization;
using ATMLibrary;

namespace ATMSimulator
{
    /// <summary>
    /// Interaction logic for SupervisorMenu.xaml
    /// </summary>
    public partial class SupervisorMenu : Window
    {
        ATMManager atm;
        Customer user;

        public SupervisorMenu(ATMManager atm, Customer user)
        {
            InitializeComponent();
            this.atm = atm;
            this.user = user;
            welcome_lb.Content += " " + user.Name;
            ShowATMInfo();
            EnableOptions(atm.InService);
        }
        private void ShowATMInfo()
        {
            atmBalance.Content = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C2}", atm.DisplayAccountBalance());
            if (atm.InService)
                atmStatus.Content = "Operational";
            else
                atmStatus.Content = "Out of Service";
        }
        private void EnableOptions(bool b)
        {
            if (atm.InService == b)
            {
                payInterest_btn.IsEnabled = b;
                refilATM_btn.IsEnabled = b;
            }
        }
        private void ChangeStatus_btn_Click(object sender, RoutedEventArgs e)
        {
            atm.ChangeATMStatus();
            EnableOptions(atm.InService);

            ShowATMInfo();
        }

        private void RefilATM_btn_Click(object sender, RoutedEventArgs e)
        {
            ValidationResponse response = atm.ATMRefil(user);
            if (response.Successful)
                ShowATMInfo();
            MessageBox.Show(response.Message);
        }

        private void PayInterest_btn_Click(object sender, RoutedEventArgs e)
        {
            atm.ATMPayInterest();
            MessageBox.Show("Interest payed.");
        }

        private void AccReport_btn_Click(object sender, RoutedEventArgs e)
        {
            AccountsReport accRep = new AccountsReport(atm.GetAccountsList());
            accRep.Show();
        }

        private void TransReport_btn_Click(object sender, RoutedEventArgs e)
        {
            TransactionsReport tRep = new TransactionsReport(atm.GetTransactionsList());
            tRep.Show();
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

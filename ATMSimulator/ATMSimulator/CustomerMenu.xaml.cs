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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Text.RegularExpressions;
using ATMLibrary;

namespace ATMSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CustomerMenu : Window
    {
        ATMManager atm;
        Customer user;
        public CustomerMenu(ATMManager atm, Customer user)
        {
            InitializeComponent();
            this.atm = atm;
            this.user = user;
            welcome_lb.Content += " " + user.Name;
            ShowBalance();
        }
        private void ShowBalance()
        {
            
            Checking c = atm.FindCheckingAcc(user.Pin);
            Savings s = atm.FindSavingsAcc(user.Pin);

            if (c != null)
            {
                cBalance_lb.Visibility = Visibility.Visible;
                cBalance.Content = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C2}", c.Balance);
            }
                
            if (s != null)
            {
                sBalance_lb.Visibility = Visibility.Visible;
                sBalance.Content = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C2}", s.Balance);
            }
                
        }
        private void Amount_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
                //Remove previous formatting, or the decimal check will fail including leading zeros
                string value = amount_textBox.Text.Replace(",", "").Replace("$", "").Replace(".", "").TrimStart('0');
                decimal ul;
                //Check we are indeed handling a number
                if (decimal.TryParse(value, out ul))
                {
                    ul /= 100;
                    //Unsub the event so we don't enter a loop
                    amount_textBox.TextChanged -= Amount_textBox_TextChanged;
                    //Format the text as currency
                    amount_textBox.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C2}", ul);
                    amount_textBox.TextChanged += Amount_textBox_TextChanged;
                    amount_textBox.Select(amount_textBox.Text.Length, 0);
                }

                bool validInput = AmountisValid(amount_textBox.Text);

                if (!validInput)
                {
                    amount_textBox.Text = "$0.00";
                    amount_textBox.Select(amount_textBox.Text.Length, 0);
                }

        }
        private bool AmountisValid(string text)
        {
            Regex money = new Regex(@"^\$(\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?$");
            return money.IsMatch(text);
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "1"; 
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "2";
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "3";
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "4";
        }

        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "5";
        }

        private void Btn6_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "6";
        }

        private void Btn7_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "7";
        }

        private void Btn8_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "8";
        }

        private void Btn9_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "9";
        }

        private void Btn0_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text += "0";
        }

        private void Clean_btn_Click(object sender, RoutedEventArgs e)
        {
            amount_textBox.Text = "";
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Submit_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double amount = double.Parse(amount_textBox.Text.Replace("$", "").TrimStart('0'));
                string transaction = "";
                if (deposit_rb.IsChecked == true)
                    transaction = deposit_rb.Tag.ToString();
                if (withdrawal_rb.IsChecked == true)
                    transaction = withdrawal_rb.Tag.ToString();
                if (transfer_rb.IsChecked == true)
                    transaction = transfer_rb.Tag.ToString();
                if (payBill_rb.IsChecked == true)
                    transaction = payBill_rb.Tag.ToString();

                char acc = 'C';
                if (checking_rb.IsChecked == true)
                   acc = 'C';
                if (savings_rb.IsChecked == true)
                   acc = 'S';

                ValidationResponse response = null;
                switch (transaction)
                {
                    case "D":
                        response = atm.ATMDeposit(amount, user, acc);
                        break;
                    case "W":
                        response = atm.ATMWithdraw(amount, user, acc);
                        break;
                    case "T":
                        response = atm.ATMTransferFunds(amount, user, acc);
                        break;
                    case "P":
                        MessageBoxResult mbResult = MessageBox.Show("This transaction has an aditional $" + atm.BillFee.ToString("F2")+" fee. Confirm?","Confirm Fee",MessageBoxButton.YesNo);
                        if (mbResult == MessageBoxResult.Yes)
                            response = atm.ATMPayBill(amount, user);
                        break;
                }

                if (response.Successful)
                {
                    ShowBalance();
                }
                MessageBox.Show(response.Message);
                amount_textBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void PayBill_rb_Checked(object sender, RoutedEventArgs e)
        {
            checking_rb.IsChecked = true;
            savings_rb.IsEnabled = false;
        }

        private void PayBill_rb_Unchecked(object sender, RoutedEventArgs e)
        {
            savings_rb.IsEnabled = true;
        }
    }
}

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
using System.Windows.Threading;
using ATMLibrary;

namespace ATMSimulator
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        ATMManager atm;
        int loginAttempts = 3;
        DispatcherTimer timer = new DispatcherTimer();
        Customer user;

        public Login()
        {
            InitializeComponent();
            atm = new ATMManager();
            if (!atm.InService)
            {
                outOfService_lb.Visibility = Visibility.Visible;
            }
        }
        void timer_Tick(object sender, System.EventArgs e)
        {
            login_btn.IsEnabled = true;
            timer.Stop();
            disableLogin_lb.Visibility = Visibility.Hidden;
            loginAttempts = 3;
        }
        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (name_textBox.Text.Equals(""))
            {
                MessageBox.Show("Must enter your name to login");
            }
            else if (pin_textBox.Password.Equals(""))
            {
                MessageBox.Show("Must enter your pin to login");
            }
            else
            {
                ValidationResponse response = atm.ValidateUser(name_textBox.Text, pin_textBox.Password);
                if (loginAttempts == 1)
                {
                    if (!response.Successful)
                    {
                        MessageBox.Show(response.Message + "\nMaximum login attempts reached. Please try again later");
                        timer.Interval = TimeSpan.FromSeconds(3);
                        timer.Tick += timer_Tick;
                        timer.Start();
                        login_btn.IsEnabled = false;
                        disableLogin_lb.Visibility = Visibility.Visible;
                    }
                }
                else if (loginAttempts > 1)
                {
                    if (!response.Successful)
                    {
                        loginAttempts--;
                        MessageBox.Show(response.Message + " Please try again");
                    }
                    else
                    {
                        user = new Customer(name_textBox.Text, pin_textBox.Password);
                        if (response.Message.Equals("Client"))
                        {
                            if(atm.InService == true)
                            {
                                CustomerMenu cmenu = new CustomerMenu(atm, user);
                                cmenu.Show();
                                cmenu.Closing += new System.ComponentModel.CancelEventHandler(MenuClosed);
                                this.Hide();
                                loginAttempts = 3;
                            }
                            else
                            {
                                MessageBox.Show("ATM is temporarily out of service. Please try again later.");
                            }
                        }
                        if (response.Message.Equals("Supervisor"))
                        {
                            SupervisorMenu smenu = new SupervisorMenu(atm, user);
                            smenu.Show();
                            smenu.Closing += new System.ComponentModel.CancelEventHandler(MenuClosed);
                            this.Hide();
                            loginAttempts = 3;
                        }

                    }
                }
                    
            }
        }
        private void MenuClosed(object sender, EventArgs e)
        {
            this.Show();
            user = null;
            name_textBox.Text = "";
            pin_textBox.Password = "";
            if (!atm.InService)
            {
                outOfService_lb.Visibility = Visibility.Visible;
            }
            else
            {
                outOfService_lb.Visibility = Visibility.Hidden;
            }
        }
    }
}

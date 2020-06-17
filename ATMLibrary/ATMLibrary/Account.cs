using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLibrary
{
    abstract public class Account {
        private string pinNumber;
        private string accountNumber;
        private double balance;
        private char accType;

        const double maxWithDrawal = 1000;
        const double maxTransfer = 100000;

        public string PinNumber { get => pinNumber; }
        public string AccountNumber { get => accountNumber; }
        public double Balance { get => balance; protected set => balance = value; }
        public char AccType { get => accType; protected set => accType = value; }

        public Account(string pinNumber, string accountNumber, double balance)
        {
            this.pinNumber = pinNumber;
            this.accountNumber = accountNumber;
            this.balance = balance;
        }

        public ValidationResponse withdraw(double amount)
        {
            bool invalid = false;
            ValidationResponse ret = new ValidationResponse();

            if (amount > maxWithDrawal)
            {
                ret.Message = "Invalid amount. Maximium withdraw value is $"+ maxWithDrawal +".";
                invalid = true;
            }
            if (amount % 10 != 0)
            {
                ret.Message = "Invalid amount. The ATM accepts only amounts multiple of $10";
                invalid = true;
            }
            if (amount > this.Balance)
            {
                ret.Message = "Inssufficient funds. Amount exceeds the account balance.";
                invalid = true;
            }

            if (invalid)
                ret.Successful = false;
            else
            {
                this.Balance -= amount;
                ret.Successful = true;
                ret.Message = "Withdraw completed.";
            }
            return ret;
        }

        public ValidationResponse deposit(double amount)
        {
            ValidationResponse ret = new ValidationResponse();
            this.Balance += amount;
            ret.Successful = true;
            ret.Message = "Deposit completed.";
            return ret;
        }

        public ValidationResponse transferOut(double amount)
        {
            bool invalid = false;
            ValidationResponse ret = new ValidationResponse();

            if (amount > maxTransfer)
            {
                ret.Message = "Invalid amount. Maximium transfer value is $"+ maxTransfer +".";
                invalid = true;
            }
            if (amount > this.Balance)
            {
                ret.Message = "Inssufficient funds. Amount exceeds the account balance.";
                invalid = true;
            }
            if (invalid)
                ret.Successful = false;
            else
            {
                this.Balance -= amount;
                ret.Successful = true;
                ret.Message = "Transfer Out completed.";
            }
            return ret;
        }

        public ValidationResponse transferIn(double amount)
        {
            bool invalid = false;
            ValidationResponse ret = new ValidationResponse();

            if (amount > maxTransfer)
            {
                ret.Message = "Invalid amount. Maximium transfer value is $" + maxTransfer + ".";
                invalid = true;
            }
            if (invalid)
                ret.Successful = false;
            else
            {
                this.Balance += amount;
                ret.Successful = true;
                ret.Message = "Transfer In completed.";
            }
            return ret;
        }

        public override string ToString()
        {
            return pinNumber + "," + accountNumber + "," + balance.ToString("F2");
        }
    }
    
    public class Checking: Account
    {
        const double maxBill = 10000;


        public Checking(string pinNumber, string accountNumber, double balance ) : base(pinNumber, accountNumber, balance)
        {
            AccType = 'C';
        }

        public ValidationResponse PayBill (double amount, double billFee)
        {
            bool invalid = false;
            ValidationResponse ret = new ValidationResponse();

            if (amount > maxBill)
            {
                ret.Message = "Invalid amount. Maximium transfer value is $"+maxBill+".";
                invalid = true;
            }
            if ((amount+billFee) > this.Balance)
            {
                ret.Message = "Inssufficient funds. Amount exceeds the account balance.";
                invalid = true;
            }
            if (invalid)
                ret.Successful = false;
            else
            {
                this.Balance -= amount+billFee;
                ret.Message = "Bill payed.";
                ret.Successful = true;
            }
            return ret;
        }
    }
    public class Savings : Account
    {
        const double interestRate = 1;

        public Savings(string pinNumber, string accountNumber, double balance) : base(pinNumber, accountNumber, balance)
        {
            AccType = 'S';
        }
       
        public void PayInterest ()
        {
            this.Balance += this.Balance * interestRate / 365 / 100;
        }
    }
    public class Bank : Account
    {
        const double maxTopUp = 20000;
        const double refillAmount = 5000;

        public double RefillAmount { get => refillAmount;}

        public Bank(string pinNumber, string accountNumber, double balance) : base(pinNumber, accountNumber, balance)
        {
            AccType = 'B';
        }

        public ValidationResponse RefilATM()
        {
            ValidationResponse ret = new ValidationResponse();
            
            //checks if the new balance exceeds the maximum allowed in the ATM, if not the transaction is successful
            if (this.Balance + RefillAmount <= maxTopUp)
            {
                this.Balance += RefillAmount;
                ret.Successful = true;
                ret.Message = "ATM Refil completed.";
            }
            else
            {
                ret.Successful = false;
                ret.Message = "ATM refil not possible. Refil exceeds the ATM maximum amount.";
            }
            return ret;
        }
    }

}

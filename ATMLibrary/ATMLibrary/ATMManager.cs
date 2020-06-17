using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLibrary
{
    public class ATMManager
    {
        List<Customer> customers;
        List<Customer> supervisors;
        List<Checking> checkingsAcc;
        List<Savings> savingsAcc;
        List<Transaction> transactionsList;

        Bank atmAcc;
        const double billFee = 1.25;
        const double depositFee = 0;
        const double withdrawalFee = 0;
        const double transferFee = 0;
        bool firstRefil;
        private bool inService = true;
        int transactionID = 1000;

        string directory = @"C:\ATM files\";

        public bool InService { get => inService; set => inService = value; }
        public double BillFee { get => billFee; }
        public double DepositFee { get => depositFee; }
        public double WithdrawalFee { get => withdrawalFee; }
        public double TransferFee { get => transferFee; }

        public ATMManager()
        {
            customers = new List<Customer>();
            supervisors = new List<Customer>();
            checkingsAcc = new List<Checking>();
            savingsAcc = new List<Savings>();
            transactionsList = new List<Transaction>();
            InService = true;
            ReadAccounts();
            ReadCustomers();
            ReadSupervisors();
            firstRefil = true;
            ATMRefil(null);
        }

        private bool ReadAccounts()
        {
            try
            {
                string path = directory + "Accounts.txt";
                
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] info = line.Split(',');
                    switch (info[0])
                    {
                        case "B":
                            atmAcc = new Bank(info[1], info[2], double.Parse(info[3]));
                            break;
                        case "C":
                            Checking c = new Checking(info[1], info[2], double.Parse(info[3]));
                            checkingsAcc.Add(c);
                            break;
                        case "S":
                            Savings s = new Savings(info[1], info[2], double.Parse(info[3]));
                            savingsAcc.Add(s);
                            break;
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
    
        }
        private bool WriteAccountsFile()
        {
            try
            {
                string path = directory + "Accounts.txt";
                
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                {
                    file.Write("B,");
                    file.WriteLine(atmAcc.ToString());
                    foreach (Checking c in checkingsAcc)
                    {
                        file.Write("C,");
                        file.WriteLine(c.ToString());
                    }
                    foreach (Savings s in savingsAcc)
                    {
                        file.Write("S,");
                        file.WriteLine(s.ToString());
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        private bool ReadCustomers()
        {
            try
            {
                string path = directory + "Customers.txt";
                
                string[] lines = System.IO.File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    string[] info = line.Split(',');

                    foreach (string i in info)
                    {
                        Customer c = new Customer(info[0], info[1]);
                        customers.Add(c);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
        }
        private bool ReadSupervisors()
        {
            try
            {
                string path = directory + "Supervisors.txt";
                
                string[] lines = System.IO.File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    string[] info = line.Split(',');

                    foreach (string i in info)
                    {
                        Customer c = new Customer(info[0], info[1]);
                        supervisors.Add(c);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        private bool WriteTransaction(Customer user, Account acc, char tCode, double amount,
                                        Bank atmBalance, double fee, string tstatus, string message)
        {
            Transaction t = new Transaction(user, acc, transactionID, tCode, amount, fee, atmAcc, tstatus, message);
            transactionsList.Add(t);

            string filename = String.Format("{0:yyyy-MM-dd}_{1}", t.TDate, "Transactions.txt");
            string path = directory + filename;

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.Write(t.ToString());
                }
                transactionID++;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        public double DisplayAccountBalance()
        {
            return atmAcc.Balance;
        }
        public ValidationResponse ValidateUser(string inputName, string inputPin)
        {
            ValidationResponse ret = new ValidationResponse();

            Customer cust = customers.Find(x => x.Pin == inputPin);
            if (cust != null)
            {
                if (cust.Name == inputName)
                {
                    ret.Successful = true;
                    ret.Message = "Client";
                    return ret;
                }
            }
            Customer sup = supervisors.Find(x => x.Pin == inputPin);
            if (sup != null)
            {
                if (sup.Name == inputName)
                {
                    ret.Successful = true;
                    ret.Message = "Supervisor";
                    return ret;
                }
            }

            ret.Successful = false;
            ret.Message = "Invalid Name or PIN.";
            return ret;
        }
        public ValidationResponse ATMWithdraw(double amount, Customer user, char type)
        {
            ValidationResponse ret = new ValidationResponse();
            if (amount > atmAcc.Balance)
            {
                ret.Message = "Amount not availiable in ATM";
                ret.Successful = false;
            }

            Account acc = null;

            if (type.Equals('C'))
            {
                acc = checkingsAcc.Find(x => x.PinNumber == user.Pin);
            }
            if (type.Equals('S'))
            {
                acc = savingsAcc.Find(x => x.PinNumber == user.Pin);
            }

            if (acc != null)
            {
                ret = acc.withdraw(amount);
                if (ret.Successful)
                {
                    ret = atmAcc.withdraw(amount);
                    if (ret.Successful)
                    {
                        WriteAccountsFile();
                        WriteTransaction(user, acc, 'W', amount, atmAcc, WithdrawalFee, "OK", ret.Message);
                        return ret;
                    }
                    else
                    {
                        WriteTransaction(user, acc, 'W', amount, atmAcc, WithdrawalFee, "NOK", ret.Message);
                        return ret;
                    }
                }
                else
                {
                    WriteTransaction(user, acc, 'W', amount, atmAcc,WithdrawalFee, "NOK", ret.Message);
                    return ret;
                }
            }
            ret.Successful = false;
            ret.Message = "No account related to PIN " + user.Pin;
            WriteTransaction(user, acc, 'W', amount, atmAcc, WithdrawalFee, "NOK", ret.Message);
            return ret;
        }
        public ValidationResponse ATMDeposit(double amount, Customer user, char type)
        {
            Account acc = null;
            ValidationResponse ret = null;
            
            if (type.Equals('C'))
            {
                acc = checkingsAcc.Find(x => x.PinNumber == user.Pin);
            }
            if (type.Equals('S'))
            {
                acc = savingsAcc.Find(x => x.PinNumber == user.Pin);
            }

            if (acc != null)
            {
                ret = acc.deposit(amount);
                if (ret.Successful)
                {
                        WriteAccountsFile();
                        WriteTransaction(user, acc, 'D', amount, atmAcc,DepositFee, "OK", ret.Message);
                        return ret;
                }
                WriteTransaction(user, acc, 'D', amount, atmAcc, DepositFee, "NOK", ret.Message);
                return ret;
            }
            ret.Successful = false;
            ret.Message = "No account related to PIN " + user.Pin;
            WriteTransaction(user, acc, 'D', amount, atmAcc, DepositFee, "NOK", ret.Message);
            return ret;
        }
        public ValidationResponse ATMPayBill(double amount, Customer user)
        {
            ValidationResponse ret = null;
            Checking acc = null;

            acc = checkingsAcc.Find(x => x.PinNumber == user.Pin);
            
            if (acc != null)
            {
                ret = acc.PayBill(amount,BillFee);
                if (ret.Successful)
                {
                    WriteAccountsFile();
                    WriteTransaction(user, acc, 'B', amount, atmAcc, BillFee, "OK", ret.Message);
                    return ret;
                }
            }
            ret.Successful = false;
            ret.Message = "No account related to PIN "+ user.Pin;
            WriteTransaction(user, acc, 'B', amount, atmAcc, BillFee, "NOK", ret.Message);
            return ret;
        }
        public ValidationResponse ATMTransferFunds(double amount, Customer user, char type)
        {
            Checking c = checkingsAcc.Find(x => x.PinNumber == user.Pin);
            Savings s = savingsAcc.Find(x => x.PinNumber == user.Pin);
            ValidationResponse ret = new ValidationResponse();

            if (c!=null)
            {
                if (s != null)
                {
                    if (type.Equals('C'))
                    {
                        if (s.transferOut(amount).Successful)
                        {
                            if (c.transferIn(amount).Successful)
                            {
                                ret.Successful = true;
                                ret.Message = "Funds transference completed.";
                                WriteAccountsFile();
                                WriteTransaction(user, c, 'T', amount, atmAcc,TransferFee, "OK", ret.Message);
                                WriteTransaction(user, s, 'T', amount, atmAcc, TransferFee, "OK", ret.Message);
                                return ret;
                            }
                        }
                        else
                        {
                            ret.Successful = false;
                            ret.Message = "Insufficient funds in savings account.";
                            WriteTransaction(user, s, 'T', amount, atmAcc, TransferFee, "NOK", ret.Message);
                            return ret;
                        }
                    }
                    if (type.Equals('S'))
                    {
                        if (c.transferOut(amount).Successful)
                        {
                            if (s.transferIn(amount).Successful)
                            {
                                ret.Successful = true;
                                ret.Message = "Funds transference completed.";
                                WriteAccountsFile();
                                WriteTransaction(user, c, 'T', amount, atmAcc, TransferFee, "OK", ret.Message);
                                WriteTransaction(user, s, 'T', amount, atmAcc, TransferFee, "OK", ret.Message);
                                return ret;
                            }   
                        }
                        else
                        {
                            ret.Successful = false;
                            ret.Message = "Insufficient funds in checking account.";
                            WriteTransaction(user, c, 'T', amount, atmAcc, TransferFee, "NOK", ret.Message);
                            return ret;
                        }
                    }
                }
                else
                {
                    ret.Successful = false;
                    ret.Message = "No savings account related to PIN "+ user.Pin;
                    WriteTransaction(user, s, 'T', amount, atmAcc, TransferFee, "NOK", ret.Message);
                    return ret;
                }
            }
            else
            {
                ret.Successful = false;
                ret.Message = "No checking account related to PIN " + user.Pin;
                WriteTransaction(user, c, 'T', amount, atmAcc, TransferFee, "NOK", ret.Message);
            }
            return ret;
        }
        public void ATMPayInterest()
        {
            foreach ( Savings s in savingsAcc)
            {
                double amount = s.Balance;
                Customer user = customers.Find(x => x.Pin == s.PinNumber);

                s.PayInterest();
                amount = s.Balance - amount;
                WriteTransaction(user, s, 'I', amount, atmAcc, 0, "OK", "Interest payed");
            }
            WriteAccountsFile();
        }
        public ValidationResponse ATMRefil(Customer user)
        {
            char tcode;
            if (firstRefil)
            {
                tcode = 'F';
                firstRefil = false;
            }
            else tcode = 'R';

            if (user == null)
            {
                user = new Customer("BANK", "0000");
            }
            ValidationResponse ret = atmAcc.RefilATM();
            if (ret.Successful)
            {
                WriteAccountsFile();
                WriteTransaction(user, atmAcc, tcode, atmAcc.RefillAmount, atmAcc,0,"OK", ret.Message);
            }
            else
            {
                WriteTransaction(user, atmAcc, tcode, atmAcc.RefillAmount, atmAcc,0, "NOK", ret.Message);
            }
            return ret;
        }
        public void ChangeATMStatus()
        {
            if (InService)
                InService = false;
            else
                InService = true;
        }
        public Checking FindCheckingAcc (string pin)
        {
            return checkingsAcc.Find(x => x.PinNumber == pin);
        }
        public Savings FindSavingsAcc(string pin)
        {
            return savingsAcc.Find(x => x.PinNumber == pin);
        }
        public List<Account> GetAccountsList()
        {
            List<Account> accountsList = new List<Account>();
            foreach (Checking c in checkingsAcc)
            {
                accountsList.Add(c);
            }
            foreach (Savings s in savingsAcc)
            {
                accountsList.Add(s);
            }
            return accountsList;
        }
        public List<Transaction> GetTransactionsList()
        {
            List<Transaction> trans = new List<Transaction>();
            foreach (Transaction t in transactionsList)
            {
                trans.Add(t);
            }
            return trans;
        }
    }
}

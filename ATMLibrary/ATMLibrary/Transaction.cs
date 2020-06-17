using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLibrary
{
    public class Transaction
    {
        Customer user; 
        Account acc; 
        DateTime tDate;
        char tCode;
        double amount;
        double fee;
        double atmBalance;
        string tStatus;
        string message;
        int transactionID;

        public DateTime TDate { get => tDate;}
        public char TCode { get => tCode; }
        public double Amount { get => amount;}
        public double Fee { get => fee;  }
        public double AtmBalance { get => atmBalance;  }
        public string TStatus { get => tStatus;  }
        public string Message { get => message;  }
        public int TransactionID { get => transactionID; }
        public Customer User { get => user; }
        public Account Acc { get => acc; }

        public Transaction(Customer user, Account acc,int transactionID, char tCode, double amount, double fee,Bank atm, string tstatus, string message)
        {
            this.user = user;
            this.acc = acc;
            this.tCode = tCode;
            this.amount = amount;
            this.atmBalance = atm.Balance;
            this.tStatus = tstatus;
            this.message = message;
            this.tDate = DateTime.Now;
            this.transactionID = transactionID;
            this.fee = fee;
        }

        public override string ToString()
        {
            string format = "F2";

            string line = transactionID + "," + tDate + "," + tCode + "," + fee + ","+ amount.ToString(format) + "," + user;
            if (acc != null)
            {
                line += "," + acc.AccType + "," + acc.AccountNumber + "," + acc.Balance.ToString(format);
            }

            line += "," + atmBalance.ToString(format) + "," + tStatus + "," + message + "\n";

            return line;
        }
    }
}

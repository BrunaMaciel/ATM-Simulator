using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLibrary
{
    public class Customer
    {
        private string name;
        private string pin;

        public string Name { get => name; }
        public string Pin { get => pin; }

        public Customer (string name, string pin)
        {
            this.name = name;
            this.pin = pin;
        }
        public override string ToString()
        {
            return name + "," + pin;
        }

    }
}

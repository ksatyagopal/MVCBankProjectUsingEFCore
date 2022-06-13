using System;
using System.Collections.Generic;

#nullable disable

namespace MVCBankProjectUsingEFCore.Models
{
    public partial class BankAccount
    {
        public BankAccount()
        {
            MoneyTransactions = new HashSet<MoneyTransaction>();
        }

        public int AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string TypeOfAccount { get; set; }
        public DateTime? DateOfCreation { get; set; }
        public double? Balance { get; set; }

        public virtual ICollection<MoneyTransaction> MoneyTransactions { get; set; }
    }
}

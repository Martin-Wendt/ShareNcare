using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Payment
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public Payment(decimal amount,string name)
        {
            Amount = amount;
            Name = name;
        }

        public override string ToString()
        {
            return Name + ", Amount: " + Amount;
        }
    }
}



//public class RequestPayment 
//{
//    internal PaymentStatus Status { get; set; } = PaymentStatus.Pending;
//    public RequestPayment(User user, decimal amount) : base(user, amount)
//    {
//    }

//    public void Pay()
//    {
//        User.Wallet.RemoveBalance(Amount);
//        Status = PaymentStatus.Completed;
//    }

//    public void Reject()
//    {
//        Status = PaymentStatus.Rejected;
//    }

//}

internal enum PaymentStatus
{
    Pending,
    Rejected,
    Completed
}



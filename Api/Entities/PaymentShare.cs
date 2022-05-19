namespace Api.Entities
{
    public class PaymentShare
    {
        public User User { get; set; }
        public User Payee { get; set; }
        public Payment Payment { get; set; }
        public decimal AmountNeeded { get; set; }
        public bool Fulfilled { get; set; } = false;

        public PaymentShare(User user, decimal amountNeeded, Payment payment, User payee)
        {
            User = user;
            Payment = payment ?? throw new ArgumentNullException(nameof(payment));
            Payee = payee ?? throw new ArgumentNullException(nameof(payee));
        }

        public override string ToString()
        {
            return User.Name + " owes " + Payee.Name + " " + AmountNeeded + " for " + Payment.Name;
        }
    }

}

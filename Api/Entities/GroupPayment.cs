namespace Api.Entities
{
    public class GroupPayment
    {
        public User Payee { get; set; }
        public Payment Payment { get; set; }
        public List<PaymentShare> Shares { get; set; } = new();
        public GroupPayment(User payee, Payment payment)
        {
            Payee = payee;
            Payment = payment;
         
        }
    }

}
